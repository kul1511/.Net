using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CROPDEAL.Models
{
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string? BankAccountNumber { get; set; }
        [Required]
        public string? BankName { get; set; }
        [Required]
        public string? IFSC_Code { get; set; }
        public Farmer Farmer { get; set; }
        [ForeignKey("Farmer")]
        public string? Farmer_Id { get; set; }
        public Dealer Dealer { get; set; }
        [ForeignKey("Dealer")]
        public string? Dealer_Id { get; set; }
    }
}