using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? PaymentId { get; set; }
        public string? OrderId { get; set; }
        public int UserId { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        [Required]
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}