using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace CROPDEAL.Models
{
    public class Farmer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Farmer_Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public IEnumerable<Crop> Crops { get; set; }
        [Required]
        [ForeignKey("BankAccount")]
        public string? BankAccountNumber { get; set; }
        [Required]
        public string? Status { get; set; }
        public DateTime Created_At { get; set; }
        public Admin Admin { get; set; }
        [ForeignKey("Admin")]
        public string? Admin_Id { get; set; }
    }
}