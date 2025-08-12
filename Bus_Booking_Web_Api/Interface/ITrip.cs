using System;
using System.Collections.Generic;
using System.Linq;
using Bus_Booking_Web_Api.Models;
using System.Threading.Tasks;

namespace Bus_Booking_Web_Api.Interface
{
    public interface ITrip
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<IEnumerable<Trip>> GetTripsByDepartureTimeAsync(DateTime departureTime);
        Task<IEnumerable<Trip>> GetTripsOrderedByBusRatingAsync();
        Task<IEnumerable<Trip>> GetTripsByBusTypeAsync(string busType);

    }
}