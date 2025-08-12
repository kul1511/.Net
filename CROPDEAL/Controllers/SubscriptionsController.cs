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
        private readonly ILogger<SubscriptionsController> log;
        private readonly ISubscription subscription;

        public SubscriptionsController(ISubscription _subscription, ILogger<SubscriptionsController> llog)
        {
            subscription = _subscription;
            log = llog;
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
                log.LogError($"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetSubscriptionById")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetSubscriptionById(int subscriptionId)
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
                log.LogError($"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddSubscription")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionDTO sub)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                    return Unauthorized("UserId not found in token.");

                int userId = int.Parse(userIdClaim.Value);

                sub.UserId = userId;
                if (!await subscription.AddSubscription(sub))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Subscription Added");
            }
            catch (Exception e)
            {
                log.LogError($"Exception Occurred: {e.Message}", DateTime.Now);
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
                log.LogError($"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("DeleteSubscription/{subscriptionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubscription(int subscriptionId)
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
                log.LogError($"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
    }
}