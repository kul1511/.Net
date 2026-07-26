using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using System.Security.Claims;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;
using System.Configuration;
using log4net;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscription subscription;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SubscriptionsController));
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
                    _logger.Warn("No subscriptions found.");
                    return NoContent();
                }
                _logger.Info($"Fetched {subscriptions.Count()} subscriptions.");
                return Ok(subscriptions);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
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
                    _logger.Warn($"Subscription not found for SubscriptionId: {subscriptionId}");
                    return NotFound();
                }
                _logger.Info($"Fetched subscription for SubscriptionId: {subscriptionId}");
                return Ok(subscriptionData);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }


        [HttpPost("SubscribeCrop")]
        [Authorize(Roles = "Dealer")]
        public async Task<IActionResult> SubscribeCrop(string CropToSubscribe, string CropId)
        {
            try
            {
                SubscriptionDTO newSub = new SubscriptionDTO
                {
                    CropType = CropToSubscribe,
                    CropId = CropId,
                    SubscriptionId = string.Concat("SB", new Random().Next(10000, 100000))
                };

                string? userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                _logger.Info($"Subscribing User Email: {userEmail}");

                if (!await subscription.AddSubscription(newSub, userEmail!))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok(new
                {
                    Message = "Successfully Subscribed ✅",
                    OrderId = newSub.SubscriptionId
                    });
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
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
                _logger.Error($"Exception Occurred: {e.Message}");
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
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}