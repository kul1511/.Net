using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bus_Booking_Web_Api.Models
{
    public class BusDTO
    {
        public string? BusNumber { get; set; }
        public int Capacity { get; set; }

        public string? Type { get; set; }
        public double Rating { get; set; }
    }
}