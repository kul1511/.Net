using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bus_Booking_Web_Api.Models
{
    public class Bus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // [JsonIgnore]
        public int Id { get; set; }
        public string? BusNumber { get; set; }
        public int Capacity { get; set; }

        public string? Type { get; set; }
        public double Rating { get; set; }
    }
}