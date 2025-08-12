using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? InvoiceId { get; set; }
        public string? OrderId { get; set; }
        public string? UserId { get; set; }
        public DateTime PaymentTime { get; set; }
        [Precision(18, 4)]
        public decimal TotalAmount { get; set; }
        [Required]
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}