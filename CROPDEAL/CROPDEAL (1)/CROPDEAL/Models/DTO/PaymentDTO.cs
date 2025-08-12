using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class PaymentDTO
    {
        public string? PaymentId { get; set; }
        public string? OrderId { get; set; }
        public string? UserId { get; set; }
        public string? PaymentMethod { get; set; }
        [JsonIgnore]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [JsonIgnore]
        public string? PaymentStatus { get; set; } = "Paid";
    }
}