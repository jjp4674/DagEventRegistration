using EventRegistration.Controllers;
using EventRegistration.Data.Context;
using EventRegistration.Data.Models;
using EventRegistration.Models;
using EventRegistration.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EventRegistration.Areas.Administration.Controllers
{
    [Authorize]
    public class RolesController : EventController
    {
        private EventContext db = new EventContext();

        // GET: Adminstration/Roles
        public ActionResult Index()
        {
            if (!ValidateCredentials(PermissionConstants.VIEW_ROLE_ADMINISTRATION))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "" });
            }

            return View(db.Roles.ToList());
        }

        // GET: Administration/Roles/Create
        public ActionResult Create()
        {
            if (!ValidateCredentials(PermissionConstants.CREATE_NEW_ROLES))
            {
                return RedirectToAction("Index");
            }

            return View(new RoleViewModel(true));
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var userId = GetCurrentUserId();

                Role newRole = new Role
                {
                    RoleId = 0,
                    Name = collection["Name"],
                    Description = collection["Description"],
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now
                };

                db.Roles.Add(newRole);
                db.SaveChanges();

                // For each selected permission, add a RolePermission record
                foreach (var key in collection.AllKeys)
                {
                    if (key.Contains(".IsSelected"))
                    {
                        if (collection[key] == "true,false")
                        {
                            RolePermission rp = new RolePermission
                            {
                                RolePermissionId = 0,
                                RoleId = newRole.RoleId,
                                PermissionId = Convert.ToInt32(key.Replace("Permission[", "").Replace("].IsSelected", "")),
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now
                            };

                            db.RolePermissions.Add(rp);
                        }
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Edit(Int64 id)
        {
            if (!ValidateCredentials(PermissionConstants.EDIT_EXISTING_ROLES))
            {
                return RedirectToAction("Index");
            }

            var role = db.Roles.FirstOrDefault(r => r.RoleId == id);
            if (role == null)
            {
                return View("Error");
            }

            return View(new RoleViewModel(id));
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                var userId = GetCurrentUserId();

                Int64 roleId = Convert.ToInt64(collection["RoleId"]);

                Role role = db.Roles.FirstOrDefault(r => r.RoleId == roleId);
                if (role == null)
                {
                    View("Error");
                }

                role.Name = collection["Name"];
                role.Description = collection["Description"];

                db.SaveChanges();

                List<int> checkedIds = new List<int>();
                // Create a list of the checked permission ids
                foreach (var key in collection.AllKeys)
                {
                    if (key.Contains(".IsSelected"))
                    {
                        var value = collection[key];

                        if (collection[key] == "true,false")
                        {
                            checkedIds.Add(Convert.ToInt32(key.Replace("Permission[", "").Replace("].IsSelected", "")));
                        }
                    }
                }

                // Find any records in the RolePermissions table that don't exist in the checked list and remove them
                foreach (RolePermission rolePermission in db.RolePermissions.Where(rp => rp.RoleId == roleId).ToList())
                {
                    // If the permission isn't found in the selected list
                    if (!checkedIds.Any(p => p == rolePermission.PermissionId))
                    {
                        db.RolePermissions.Remove(rolePermission);
                    }
                }

                // For each selected permission, add a rolePermission record if it doesn't exist already
                foreach (int permissionId in checkedIds)
                {
                    var rpDB = db.RolePermissions.FirstOrDefault(rps => rps.RoleId == roleId && rps.PermissionId == permissionId);
                    if (rpDB == null)
                    {
                        RolePermission rp = new RolePermission
                        {
                            RolePermissionId = 0,
                            RoleId = roleId,
                            PermissionId = permissionId,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        };

                        db.RolePermissions.Add(rp);
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}