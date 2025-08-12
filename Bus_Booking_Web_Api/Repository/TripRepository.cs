using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus_Booking_Web_Api.Data;
using Bus_Booking_Web_Api.Interface;
using Bus_Booking_Web_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_Web_Api.Repository
{
    public class TripRepository : ITrip
    {
        private readonly BusBookingContext b;
        public TripRepository(BusBookingContext _bus)
        {
            b = _bus;
        }

        async Task<IEnumerable<Trip>> ITrip.GetTripsByDepartureTimeAsync(DateTime departureTime)
        {
            return await b.Trips.Include(b => b.Bus).Where(t => t.DepartureTime == departureTime).ToListAsync();
        }

        async Task<IEnumerable<Trip>> ITrip.GetTripsOrderedByBusRatingAsync()
        {
            return await b.Trips.Include(b => b.Bus).OrderByDescending(t => t.Bus.Rating).ToListAsync();
        }

        async Task<IEnumerable<Trip>> ITrip.GetTripsByBusTypeAsync(string busType)
        {
            return await b.Trips.Include(b => b.Bus).Where(t => t.Bus.Type == busType).ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await b.Trips.ToListAsync();
        }
    }
}