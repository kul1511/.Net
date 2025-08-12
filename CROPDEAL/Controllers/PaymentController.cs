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
        private readonly ILogger<PaymentController> log;

        public PaymentController(IPayment _payment, ILogger<PaymentController> llog)
        {
            payment = _payment;
            log = llog;
        }

        // [HttpGet("GetAllPayments")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> GetAllPayments()
        // {
        //     try
        //     {
        //         var payments = await payment.GetAllPayments();
        //         return Ok(payments);
        //     }
        //     catch (Exception ex)
        //     {
        //         await payment.LogToDatabase("Error", $"Exception Occurred: {ex.StackTrace}", DateTime.Now);
        //         return BadRequest(ex.StackTrace);
        //     }
        // }

        // [HttpGet("GetPayment/{orderId}")]
        // [Authorize(Roles = "Dealer,Admin")]
        // public async Task<IActionResult> GetPaymentByOrderId(string orderId)
        // {
        //     try
        //     {
        //         var payment1 = await payment.GetPaymentByOrderId(orderId);
        //         if (payment1 == null)
        //         {
        //             return NotFound("Payment Not Found");
        //         }
        //         return Ok(payment1);
        //     }
        //     catch (Exception ex)
        //     {
        //         await payment.LogToDatabase("Error", $"Exception Occurred: {ex.StackTrace}", DateTime.Now);
        //         return BadRequest(ex.StackTrace);
        //     }
        // }

        [HttpPost("MakePayment/{orderId}")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> MakePayment(string orderId)
        {
            try
            {
                var paymentRes = await payment.MakePayment(orderId);
                return Ok(paymentRes);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetPaymentStatus/{paymentId}")]
        public async Task<IActionResult> GetPaymentStatus(string paymentId)
        {
            try
            {
                var paymentRes = await payment.CheckPaymentStatus(paymentId);
                return Ok(paymentRes);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpPost("MarkPaymentSuccess/{paymentId}")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> MarkPaymentSuccess(string paymentId)
        {
            try
            {
                var result = await payment.MarkPaymentSuccess(paymentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }


    }
}