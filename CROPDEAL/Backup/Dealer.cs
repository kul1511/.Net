using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CROPDEAL.Models
{
    public class Dealer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Dealer_Id { get; set; }
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
        public DateTime Created_At { get; set; }
        public IEnumerable<Crop> Subscribed_Crops { get; set; }
        public Crop Crop { get; set; }
        [ForeignKey("Crop")]
        public int Crop_Id { get; set; }
        [ForeignKey("BankAccount")]
        [Required]
        public string? BankAccountNumber { get; set; }
        public Admin Admin { get; set; }
        [ForeignKey("Admin")]
        public string? Admin_Id { get; set; }
    }
}