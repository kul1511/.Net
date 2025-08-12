using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CROPDEAL.Services;

namespace CROPDEAL.Models
{
    public enum UserRole
    {
        Farmer = 0,
        Dealer = 1,
        Admin = 2
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int UserId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        [PasswordValidation]
        public string? Password_Hash { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole? Role { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}