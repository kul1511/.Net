using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Models
{
    public class Crop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? CropId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string? CropType { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 4)]
        public decimal PricePerUnit { get; set; }
        public string? Location { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}