using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CROPDEAL.Services;

namespace CROPDEAL.Models.DTO
{
    public class UserViewDTO
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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole? Role { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}