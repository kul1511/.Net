using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SubscriptionsController : ControllerBase
    {
        // private RegisterRepository register;
        private readonly ISubscription subscription;

        public SubscriptionsController(ISubscription _subscription)
        {
            subscription = _subscription;
        }
        [HttpGet("GetAllSubscriptions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            try
            {
                var subscriptions = await subscription.GetAllSubscriptions();
                if (subscriptions == null)
                {
                    return NoContent();
                }
                return Ok(subscriptions);
            }
            catch (Exception e)
            {
                await subscription.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetSubscriptionById")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetSubscriptionById(string subscriptionId)
        {
            try
            {
                var subscriptionData = await subscription.GetSubscriptionById(subscriptionId);
                if (subscriptionData == null)
                {
                    return NoContent();
                }
                return Ok(subscriptionData);
            }
            catch (Exception e)
            {
                await subscription.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddSubscription")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionDTO sub)
        {
            try
            {
                if (!await subscription.AddSubscription(sub))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Subscription Added");
            }
            catch (Exception e)
            {
                await subscription.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
        [HttpPut("UpdateSubscription")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> UpdateSubscription([FromBody] SubscriptionDTO sub)
        {
            try
            {
                if (!await subscription.UpdateSubscription(sub))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Subscription Updated");
            }
            catch (Exception e)
            {
                await subscription.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("DeleteSubscription/{subscriptionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubscription(string subscriptionId)
        {
            try
            {
                if (!await subscription.DeleteSubscription(subscriptionId))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Subscription Deleted");
            }
            catch (Exception e)
            {
                await subscription.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
    }
}