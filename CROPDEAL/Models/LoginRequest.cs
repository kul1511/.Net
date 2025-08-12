using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Services;

namespace CROPDEAL.Models
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string? Email { get; set; }
        [PasswordValidation]
        public string? Password_Hash { get; set; }
    }
}