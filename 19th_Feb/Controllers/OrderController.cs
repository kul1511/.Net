using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;
using System.Security.Claims;
using log4net;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrders crop;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderController));
        public OrderController(IOrders _crop)
        {
            crop = _crop;
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
                    _logger.Warn("No orders found.");
                    return NoContent();
                }
                _logger.Info($"Fetched {Orders.Count()} orders.");
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
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
                    _logger.Warn($"Order not found for OrderId: {orderId}");
                    return NotFound();
                }
                _logger.Info($"Fetched order for OrderId: {orderId}");
                return Ok(OrdersData);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("MakeOrder")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakeOrder(string cropId, int quantity, string subscriptionId)
        {
            try
            {
                var currentUserEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                if (currentUserEmail == null || string.IsNullOrEmpty(currentUserEmail))
                {
                    _logger.Warn($"Unable to get the Current User Email");
                }
                    _logger.Info($"Current User Email: {currentUserEmail}");

                OrderDTO newOrder = new OrderDTO
                {
                    CropId = cropId,
                    OrderDate = DateTime.Now,
                    OrderId = string.Concat("O", new Random().Next(10000, 100000)),
                    Quantity = quantity,
                    SubscriptionId = subscriptionId
                };

                if (await crop.AddOrder(newOrder, currentUserEmail!))
                {
                    return Ok( new
                    {
                        Message = "Successfully Made Order✅",
                        OrderId = newOrder.OrderId
                    });
                }
                return BadRequest("Wrong Request");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.StackTrace}");
                return BadRequest(ex.Message);
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
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
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
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrdersByDealerId/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersByDealerId(string userId)
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
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
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

            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}