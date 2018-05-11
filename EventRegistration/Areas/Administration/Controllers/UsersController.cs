using EventRegistration.Controllers;
using EventRegistration.Data.Context;
using EventRegistration.Data.Models;
using EventRegistration.Mail;
using EventRegistration.Models;
using EventRegistration.Security;
using EventRegistration.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventRegistration.Areas.Administration.Controllers
{
    [Authorize]
    public class UsersController : EventRegController
    {
        private EventContext db = new EventContext();

        // GET: Administration/Users
        public ActionResult Index()
        {
            if (!ValidateCredentials(PermissionConstants.VIEW_USER_ADMINISTRATION))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "" });
            }

            return View(db.Users.ToList());
        }

        // GET: Administration/Users/Create
        public ActionResult Create()
        {
            if (!ValidateCredentials(PermissionConstants.CREATE_NEW_USERS))
            {
                return RedirectToAction("Index");
            }

            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                User newUser = new User
                {
                    UserId = 0,
                    Username = model.Username,
                    Email = model.Email,
                    EmailToken = token,
                    EmailConfirmed = false,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Locked = false,
                    FailedLogins = 0,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                foreach (var role in model.Roles.Where(r => r.IsSelected))
                {
                    UserRole newUserRole = new UserRole
                    {
                        UserRoleId = 0,
                        UserId = newUser.UserId,
                        RoleId = role.RoleId,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now
                    };

                    db.UserRoles.Add(newUserRole);
                }
                db.SaveChanges();

                // Send email confirmation to new user
                string link = "https://" + Request.Url.Authority + "/Administration/Users/ConfirmEmail?username=" + model.Username + "&token=" + Url.Encode(token);
                string mailBody = "<p>Hello " + model.FirstName + ",</p>";
                mailBody += "<p>An account has been created for you in your company's Castel DVR.  Please follow the link below to confirm your email and set your password:</p>";
                mailBody += "<p><a href=\"" + link + "\">" + link + "</a></p>";
                mailBody += "<p>Thank you!</p>";
                //MailSender.SendEmail("jjp4674@gmail.com", model.Email, "Dagorhir Event Registration - User Email Confirmation Required", mailBody);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string username, string token)
        {
            if (username == null || token == null)
            {
                return View("Error");
            }

            var result = UserFunctions.ConfirmEmail(username, token);
            if (!result)
            {
                return View("Error");
            }

            var user = db.Users.FirstOrDefault(u => u.Username == username && u.EmailToken == token);
            if (user == null)
            {
                return View("Error");
            }

            // If the user hasn't already set a password, show the password set panel
            if (String.IsNullOrEmpty(user.PasswordHash))
            {
                RegisterViewModel rvm = new RegisterViewModel
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return View(rvm);
            }
            else
            {
                RegisterViewModel rvm = new RegisterViewModel
                {
                    UserId = 0
                };

                return View(rvm);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ConfirmEmail(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user != null)
                {
                    user.PasswordHash = PasswordFunctions.HashPassword(model.Password);
                    user.EmailToken = null;

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Edit(Int64 id)
        {
            if (!ValidateCredentials(PermissionConstants.EDIT_EXISTING_USERS))
            {
                return RedirectToAction("Index");
            }

            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return View("Error");
            }

            return View(new UserViewModel(user));
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                User user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user == null)
                {
                    return View("Error");
                }

                bool newEmail = false;
                var token = "";
                if (user.Email != model.Email)
                {
                    user.EmailConfirmed = false;
                    token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    user.EmailToken = token;
                    newEmail = true;
                }
                user.Email = model.Email;

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                // If locked and being unlocked, reset the failed logins count
                if (!model.Locked && user.Locked != model.Locked)
                {
                    user.FailedLogins = 0;
                }
                // If unlocked and being locked, set the locked date
                if (model.Locked && user.Locked != model.Locked)
                {
                    user.LockedDate = DateTime.Now;
                }
                user.Locked = model.Locked;

                user.UpdatedBy = userId;
                user.UpdatedDate = DateTime.Now;
                db.SaveChanges();

                // Find any records in the UserRecords table that don't exist in the checked list and remove them
                foreach (UserRole userRole in db.UserRoles.Where(ur => ur.UserId == user.UserId).ToList())
                {
                    // If the role isn't found in the selected list
                    if (!model.Roles.Any(r => r.RoleId == userRole.RoleId && r.IsSelected))
                    {
                        db.UserRoles.Remove(userRole);
                    }
                }

                // For each selected role on the page, add a userrole record if it doesn't exist
                foreach (RoleViewModel rvm in model.Roles.Where(r => r.IsSelected).ToList())
                {
                    var urDB = db.UserRoles.FirstOrDefault(urs => urs.UserId == user.UserId && urs.RoleId == rvm.RoleId);
                    if (urDB == null)
                    {
                        UserRole ur = new UserRole
                        {
                            UserRoleId = 0,
                            UserId = user.UserId,
                            RoleId = rvm.RoleId,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        };

                        db.UserRoles.Add(ur);
                    }
                }
                db.SaveChanges();

                if (newEmail)
                {
                    // Send email confirmation to new user
                    string link = "https://" + Request.Url.Authority + "/Administration/Users/ConfirmEmail?username=" + user.Username + "&token=" + token;
                    string mailBody = "<p>Hello " + user.FirstName + ",</p>";
                    mailBody += "<p>Your account's email address has been changed in your company's Castel DVR.  Please follow the link below to confirm your email:</p>";
                    mailBody += "<p><a href=\"" + link + "\">" + link + "</a></p>";
                    mailBody += "<p>Thank you!</p>";
                    //MailSender.SendEmail("jjp4674@gmail.com", user.Email, "Dagorhir Event Registration - User Email Confirmation Required", mailBody);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Int64 userId)
        {
            if (!ValidateCredentials(PermissionConstants.DELETE_USERS))
            {
                return RedirectToAction("Index");
            }

            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                // Remove user roles
                foreach (var userRole in db.UserRoles.Where(ur => ur.UserId == userId).ToList())
                {
                    db.UserRoles.Remove(userRole);
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(Int64 userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.PasswordToken = token;
                db.SaveChanges();

                // Send email confirmation to new user
                string link = "https://" + Request.Url.Authority + "/Administration/Users/ChangePassword?username=" + user.Username + "&token=" + Url.Encode(token);
                string mailBody = "<p>Hello " + user.FirstName + ",</p>";
                mailBody += "<p>A password reset has been triggered for your account in your company's Castel DVR.  Please follow the link below to set your new password:</p>";
                mailBody += "<p><a href=\"" + link + "\">" + link + "</a></p>";
                mailBody += "<p>Thank you!</p>";
                //MailSender.SendEmail("jjp4674@gmail.com", user.Email, "Dagorhir Event Registration - User Password Change Required", mailBody);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult ChangePassword(string username, string token)
        {
            if (username == null || token == null)
            {
                return View("Error");
            }

            var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordToken == token);
            if (user == null)
            {
                return View("Error");
            }

            PasswordViewModel pvm = new PasswordViewModel
            {
                UserId = user.UserId
            };

            return View(pvm);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user != null)
                {
                    // Check if the new password is the same as the old password
                    if (PasswordFunctions.ValidatePassword(model.Password, user.PasswordHash))
                    {
                        ModelState.AddModelError(string.Empty, "The new password must not be a password that has already been used.");
                        return View(model);
                    }

                    user.PasswordHash = PasswordFunctions.HashPassword(model.Password);
                    user.PasswordToken = null;

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}