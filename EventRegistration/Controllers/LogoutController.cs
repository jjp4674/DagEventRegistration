using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace EventRegistration.Controllers
{
    public class LogoutController : Controller
    {

        public ActionResult Index()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Login");
        }
    }
}