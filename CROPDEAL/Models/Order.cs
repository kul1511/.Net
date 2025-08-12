using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? OrderId { get; set; }

        public string? OrderNumber { get; set; }
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        public int UserId { get; set; }

        public string? CropId { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string? Status { get; set; }
        [ForeignKey("CropId")]
        public Crop? Crop { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}