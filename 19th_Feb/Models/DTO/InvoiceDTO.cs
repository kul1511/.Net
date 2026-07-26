using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class InvoiceDTO
    {
        public string? InvoiceId { get; set; }
        public string? OrderId { get; set; }
        public string? UserId { get; set; }
        public string? PaymentId { get; set; }
        // public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? InvoiceStatus { get; set; }
    }
}