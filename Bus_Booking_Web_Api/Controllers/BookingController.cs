using Bus_Booking_Web_Api.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Booking_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBooking b;
        public BookingController(IBooking _b)
        {
            b = _b;
        }

        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            return Ok(await b.GetAllBookingsAsync());
        }

        [HttpGet("GetByPassengerName/{passengerName}")]
        public async Task<IActionResult> GetByPassengerName([FromRoute] string passengerName)
        {
            var res = await b.GetBookingsByPassengerNameAsync(passengerName);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }

        [HttpGet("GetByTripDate/{startDate}/{endDate}")]
        public async Task<IActionResult> GetByPassengerName([FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
        {
            var res = await b.GetBookingsByTripDateRangeAsync(startDate, endDate);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }

        [HttpGet("GetByOriginDestination/{origin}/{destination}")]
        public async Task<IActionResult> GetByOriginDestination([FromRoute] string origin, [FromRoute] string destination)
        {
            var res = await b.GetBookingsByTripOriginAndDestinationAsync(origin, destination);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }
    }
}