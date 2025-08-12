using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bus_Booking_Web_Api.Data;
using Bus_Booking_Web_Api.Models;
using Bus_Booking_Web_Api.Interface;
using Bus_Booking_Web_Api.Repository;
using Microsoft.Identity.Client;


namespace Bus_Booking_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly IBus b;
        public BusController(IBus _bus)
        {
            b = _bus;
        }

        [HttpGet("GetAllBuses")]
        public async Task<IActionResult> GetAllBuses()
        {
            var res = await b.GetAllBusesAsync();
            if (res != null) return Ok(res);
            return NotFound();
        }

        [HttpPost("AddBus")]
        public async Task<IActionResult> AddBus([FromBody] Bus bus)
        {
            bus.Id = 0;
            var res = await b.AddBusAsync(bus);
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }

        [HttpPut("UpdateBus/{id}")]
        public async Task<IActionResult> UpdateBus([FromRoute] int id, [FromBody] Bus bus)
        {
            if (await b.UpdateBusAsync(id, bus))
            {
                return Ok("Updated");
            }
            return NotFound();
        }

        [HttpDelete("DeleteBus/{id}")]
        public async Task<IActionResult> DeleteBus([FromRoute] int id)
        {
            if (await b.DeleteBusAsync(id))
            {
                return Ok("Deleted");
            }
            return NotFound();
        }
    }
}