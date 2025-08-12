using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models;
using CROPDEAL.Repository;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment payment;
        // private RegisterRepository register;

        public PaymentController(IPayment _payment)
        {
            payment = _payment;
        }

        [HttpGet("GetAllPayments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await payment.GetAllPayments();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                await payment.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPayment/{orderId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetPaymentByOrderId(string orderId)
        {
            try
            {
                var payment1 = await payment.GetPaymentByOrderId(orderId);
                if (payment1 == null)
                {
                    return NotFound("Payment Not Found");
                }
                return Ok(payment1);
            }
            catch (Exception ex)
            {
                await payment.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MakePayment")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentDTO paymentDTO)
        {
            try
            {
                if (await payment.MakePayment(paymentDTO))
                {
                    return Ok("Payment Added Successfully");
                }
                return BadRequest("Failed to Add Payment");
            }
            catch (Exception ex)
            {
                await payment.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

    }
}