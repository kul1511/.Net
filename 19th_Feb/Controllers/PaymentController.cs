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
using Razorpay.Api;
using log4net;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment payment;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PaymentController));
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
                _logger.Error($"Exception Occurred: {ex.Message}");
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
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("MakePayment")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakePayment(string OrderId)
        {
            try
            {
                PaymentDTO paymentDTO = new PaymentDTO
                {
                    PaymentId = Guid.NewGuid().ToString(),
                    OrderId = OrderId,
                    PaymentDate = DateTime.Now
                };

                var order = await payment.CreateOrder(200000);

                _logger.Info($"Response Received from Razorpay API:Id={order["id"]}, Amount={order["amount"]}, Currency={order["currency"]}, Status={order["status"]}");

                // Save to DB using EF
                // var paymentOrder = new PaymentOrder
                // {
                //     RazorpayOrderId = order["id"].ToString(),
                //     Amount = amount,
                //     Status = "Created",
                //     CreatedAt = DateTime.UtcNow
                // };
                // _context.PaymentOrders.Add(paymentOrder);
                // _context.SaveChanges();


                if (await payment.MakePayment(paymentDTO))
                {
                    return Ok(new
                    {
                        orderId = paymentDTO.OrderId,
                        PaymentId = paymentDTO.PaymentId,
                        amount = paymentDTO.Amount
                    });
                }
                return BadRequest("Failed to Add Payment");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        // [HttpPost("verify-payment")]
        // public IActionResult VerifyPayment([FromBody] PaymentVerificationDto dto)
        // {
        //     var generatedSignature = Utils.CalculateRFC2104HMAC(
        //         dto.RazorpayOrderId + "|" + dto.RazorpayPaymentId,
        //         _config["Razorpay:Secret"]
        //     );

        //     if (generatedSignature == dto.RazorpaySignature)
        //     {
        //         var order = _context.PaymentOrders.FirstOrDefault(o => o.RazorpayOrderId == dto.RazorpayOrderId);
        //         if (order != null)
        //         {
        //             order.Status = "Paid";
        //             order.PaymentId = dto.RazorpayPaymentId;
        //             order.Signature = dto.RazorpaySignature;
        //             _context.SaveChanges();
        //         }
        //         return Ok("Payment Verified");
        //     }
        //     return BadRequest("Invalid Signature");
        // }

    }
}