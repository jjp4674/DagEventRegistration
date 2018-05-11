using EventRegistration.Data.Context;
using System.Linq;

namespace EventRegistration.Security
{
    public static class UserFunctions
    {
        public static bool ConfirmEmail(string username, string token)
        {
            using (EventContext db = new EventContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user == null || user.EmailToken != token)
                {
                    return false;
                }

                user.EmailConfirmed = true;
                db.SaveChanges();

                return true;
            }
        }
    }
}