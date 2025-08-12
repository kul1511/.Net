using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class CropDTO
    {
        public string? CropId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public string? CropType { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public string? Location { get; set; }
    }
}