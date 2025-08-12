using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Bus_Booking_Web_Api.Models
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public decimal Fare { get; set; }
        public Bus Bus { get; set; }
        [ForeignKey("Bus")]
        public int BusId { get; set; }

    }
}