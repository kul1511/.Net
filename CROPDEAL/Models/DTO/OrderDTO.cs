using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Models.DTO
{
    public class OrderDTO
    {
        public string? OrderId { get; set; }
        [JsonIgnore]
        public string? OrderNumber { get; set; }
        public string? CropId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        // public decimal Price { get; set; }
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        // [JsonIgnore]
        public DateTime OrderDate { get; set; }

        // public string? Status { get; set; }
    }
}