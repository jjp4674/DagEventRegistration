namespace EventRegistration.Security
{
    public class PasswordFunctions
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string dbHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, dbHash);
        }
    }
}