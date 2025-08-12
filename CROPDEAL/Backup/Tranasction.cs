using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CROPDEAL.Models
{
    public class Tranasction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int Transaction_Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Total_Price { get; set; }
        [Required]
        public string? Payment_Status { get; set; }

    }
}