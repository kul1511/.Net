using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPDEAL.Models.DTO
{
    public class CropDTO
    {
        public string? CropId { get; set; }
        public string? UserId { get; set; }
        public string? CropType { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public string? Location { get; set; }
    }
}