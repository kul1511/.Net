using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace CROPDEAL.Services
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Password is required.");
            }

            string password = value.ToString();

            string pattern = @"^(?=.*[A-Z])(?=.*[@$!%*?&]).{6,}$";

            if (!Regex.IsMatch(password, pattern))
            {
                return new ValidationResult("Password must be at least 6 characters long, contain an uppercase letter, and a special character (@, #, $, etc.).");
            }

            return ValidationResult.Success;

        }
    }
}
