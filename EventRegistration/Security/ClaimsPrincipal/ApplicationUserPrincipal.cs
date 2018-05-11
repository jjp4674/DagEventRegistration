using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EventRegistration.Security
{
    public class ApplicationUserPrincipal : ClaimsPrincipal
    {
        public ApplicationUserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {

        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }

        public List<Claim> Roles
        {
            get
            {
                return this.FindAll(ClaimTypes.Role).ToList();
            }
        }
    }
}