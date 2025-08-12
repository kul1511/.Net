using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus_Booking_Web_Api.Models;
using Bus_Booking_Web_Api.Interface;
using Bus_Booking_Web_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Bus_Booking_Web_Api.Repository
{
    public class BusRepository : IBus
    {
        private readonly BusBookingContext b;
        public BusRepository(BusBookingContext _bus)
        {
            b = _bus;
        }
        public async Task<IEnumerable<Bus>> GetAllBusesAsync()
        {
            return await b.Buses.ToListAsync();
            // return buses;
        }
        public async Task<Bus> AddBusAsync(Bus bus)
        {
            var res = await b.Buses.FirstOrDefaultAsync(b => b.Id == bus.Id);
            if (res == null)
            {
                await b.Buses.AddAsync(bus);
                await b.SaveChangesAsync();
                return bus;
            }
            return res;
        }
        public async Task<bool> UpdateBusAsync(int id, Bus bus)
        {
            var res = await b.Buses.FirstOrDefaultAsync(b => b.Id == id);
            if (res != null)
            {
                res.BusNumber = bus.BusNumber;
                res.Capacity = bus.Capacity;
                res.Rating = bus.Rating;
                res.Type = bus.Type;
                await b.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteBusAsync(int id)
        {
            var res = await b.Buses.FirstOrDefaultAsync(b => b.Id == id);
            if (res != null)
            {
                b.Buses.Remove(res);
                await b.SaveChangesAsync();
                return true;
            }
            return false;

        }
    }
}