using System;
using Bus_Booking_Web_Api.Data;
using Bus_Booking_Web_Api.Interface;
using Bus_Booking_Web_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_Web_Api.Repository
{
    public class BookingRepository : IBooking
    {
        private readonly BusBookingContext b;
        public BookingRepository(BusBookingContext _b)
        {
            b = _b;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await b.Bookings.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPassengerNameAsync(string passengerName)
        {
            return await b.Bookings.Include(t => t.Trip).Where(b => b.PassengerName == passengerName).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTripDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await b.Bookings.Include(t => t.Trip).Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTripOriginAndDestinationAsync(string origin, string destination)
        {
            return await b.Bookings.Include(t => t.Trip).Where(t => t.Trip.Origin == origin && t.Trip.Destination == destination).ToListAsync();
        }
    }
}