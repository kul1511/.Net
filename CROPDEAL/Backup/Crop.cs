using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CROPDEAL.Models
{
    public class Crop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int Crop_Id { get; set; }
        [Required]
        public string? Crop_Name { get; set; }
        [Required]
        public string? Crop_Type { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price_Per_Unit { get; set; }
        [Required]
        public string? Location { get; set; }
        public DateTime Created_At { get; set; }
        public Farmer Farmer { get; set; }
        [ForeignKey("Farmer")]
        public string? Farmer_Id { get; set; }
    }
}