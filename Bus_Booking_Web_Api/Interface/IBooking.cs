using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus_Booking_Web_Api.Models;

namespace Bus_Booking_Web_Api.Interface
{
    public interface IBooking
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByPassengerNameAsync(string passengerName);
        Task<IEnumerable<Booking>> GetBookingsByTripDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetBookingsByTripOriginAndDestinationAsync(string origin, string destination);

    }
}