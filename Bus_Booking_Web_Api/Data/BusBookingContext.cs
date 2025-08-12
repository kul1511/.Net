using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bus_Booking_Web_Api.Models;

namespace Bus_Booking_Web_Api.Data
{
    public class BusBookingContext : DbContext
    {
        public BusBookingContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}