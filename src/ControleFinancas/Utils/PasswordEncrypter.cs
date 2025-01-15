namespace AppointmentsManager.Utils
{
    public class PasswordEncrypter
    {
        private const int SaltRounds = 12;
        public string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(SaltRounds);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
