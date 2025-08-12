using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging.EventLog;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> log;
        private readonly IOrders crop;
        public OrderController(IOrders _crop, ILogger<OrderController> llog)
        {
            crop = _crop;
            log = llog;
        }
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var Orders = await crop.GetAllOrders();
                if (Orders == null)
                {
                    return NoContent();
                }
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetOrderByOrderId/{orderId}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
        {
            try
            {
                var OrdersData = await crop.GetOrderById(orderId);
                if (OrdersData == null)
                {
                    return NoContent();
                }
                return Ok(OrdersData);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }



        [HttpPost("MakeOrder")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakeOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                    return Unauthorized("UserId not found in token.");

                int userId = int.Parse(userIdClaim.Value);

                orderDTO.UserId = userId;

                if (await crop.AddOrder(orderDTO))
                {
                    return Ok("Order Added");
                }
                return BadRequest("Wrong Request");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
        [HttpPut("UpdateOrder/{id}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] OrderDTO orderDTO)
        {
            try
            {
                if (await crop.UpdateOrder(id, orderDTO))
                {
                    return Ok("Order Updated Successfully");
                }
                return NotFound("Order Not Found");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
        [HttpDelete("CancelOrder/{id}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            try
            {
                if (await crop.DeleteOrder(id))
                {
                    return Ok("Order Deleted Successfully");
                }
                return NotFound("Order Not Found");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetOrdersByDealerId/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersByDealerId(int userId)
        {
            try
            {
                var cropsForUser = await crop.GetOrdersByUserId(userId);
                if (cropsForUser == null)
                {
                    return NoContent();
                }
                return Ok(cropsForUser);
            }

            catch (Exception e)
            {
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GenerateReportByDate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateReportByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var cropsForUser = await crop.GetOrdersWithinDateRange(startDate, endDate);
                if (cropsForUser == null)
                {
                    return NoContent();
                }
                return Ok(cropsForUser);
            }

            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
    }
}