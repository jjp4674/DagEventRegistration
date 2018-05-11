using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace EventRegistration.Security
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected ApplicationUserPrincipal CurrentUser
        {
            get
            {
                return new ApplicationUserPrincipal(this.User as ClaimsPrincipal);
            }
        }

        protected bool CheckPermission(string permissionName)
        {
            if (CurrentUser.Claims.Any(c => c.Value.Contains(permissionName)))
            {
                return true;
            }

            return false;
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {

    }
}