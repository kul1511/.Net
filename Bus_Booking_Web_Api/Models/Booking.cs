using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Booking_Web_Api.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? PassengerName { get; set; }
        public string? PassengerEmail { get; set; }
        public DateTime BookingDate { get; set; }
        public Trip Trip { get; set; }
        [ForeignKey("Trip")]
        public int TripId { get; set; }

    }
}