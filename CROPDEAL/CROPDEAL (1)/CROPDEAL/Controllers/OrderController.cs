using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrders crop;
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
                    return NoContent();
                }
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                await crop.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
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
                    return NoContent();
                }
                return Ok(OrdersData);
            }
            catch (Exception ex)
            {
                await crop.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("MakeOrder")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakeOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                if (await crop.AddOrder(orderDTO))
                {
                    return Ok("Order Added");
                }
                return BadRequest("Wrong Request");
            }
            catch (Exception ex)
            {
                await crop.LogToDatabase("Error", $"Exception Occurred: {ex.StackTrace}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
    }
}