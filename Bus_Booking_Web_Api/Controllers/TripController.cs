using Microsoft.AspNetCore.Mvc;
using Bus_Booking_Web_Api.Interface;
using System.Security.Cryptography;

namespace Bus_Booking_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITrip t;
        public TripController(ITrip _t)
        {
            t = _t;
        }

        [HttpGet("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips()
        {
            return Ok(await t.GetAllTripsAsync());
        }

        [HttpGet("FilterByDepartureTime")]
        public async Task<IActionResult> FilterByDepartureTime([FromQuery] DateTime departureTime)
        {
            var res = await t.GetTripsByDepartureTimeAsync(departureTime);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }

        [HttpGet("FilterByBusRating")]
        public async Task<IActionResult> FilterByBusRating()
        {
            var res = await t.GetTripsOrderedByBusRatingAsync();
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }

        [HttpGet("FilterByBusType/{busType}")]
        public async Task<IActionResult> FilterByBusType([FromRoute] string busType)
        {
            var res = await t.GetTripsByBusTypeAsync(busType);
            if (res != null)
            {
                return Ok(res);
            }
            return NotFound();
        }
    }
}