using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models
{
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? BankId { get; set; }
        public string? UserId { get; set; }
        public long AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? IFSCCode { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}