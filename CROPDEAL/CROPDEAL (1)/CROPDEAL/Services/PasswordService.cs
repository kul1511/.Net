using Microsoft.AspNetCore.Identity;

namespace CROPDEAL.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}