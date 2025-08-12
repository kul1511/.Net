using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus_Booking_Web_Api.Models;

namespace Bus_Booking_Web_Api.Interface
{
    public interface IBus
    {
        Task<IEnumerable<Bus>> GetAllBusesAsync();
        Task<Bus> AddBusAsync(Bus bus);
        Task<bool> UpdateBusAsync(int id, Bus bus);
        Task<bool> DeleteBusAsync(int id);
    }
}