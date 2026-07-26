using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;
using Stripe.Terminal;
using Stripe;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using CROPDEAL.Data;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace CROPDEAL.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CropsController : ControllerBase
    {
        private readonly ICrops crop;
        private readonly CropDealDbContext context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CropsController));
        public CropsController(ICrops _crop, CropDealDbContext _context)
        {
            crop = _crop;
            context = _context;
        }

        [HttpGet("GetAllCrops")]
        [Authorize(Roles = "Admin,Dealer")]
        public async Task<IActionResult> GetAllCrops()
        {
            try
            {
                var Crops = await crop.GetAllCrops();
                if (Crops == null)
                {
                    return NoContent();
                }
                return Ok(Crops);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetCropById")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetCropById(string cropId)
        {
            try
            {
                var Crops = await crop.GetCropById(cropId);
                if (Crops == null)
                {
                    return NoContent();
                }
                return Ok(Crops);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddCrop")]
        [Authorize(Roles = "Farmer,Admin")]
        public async Task<IActionResult> AddCrop(string cropType, string location, decimal pricePerUnit, int quantity, string userEmail)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(c => c.Email == userEmail);
                if (user == null)
                {
                    _logger.Warn("Farmer Not Found while Adding Crop");
                    return NotFound("Unable to add Crop. User Not Found");
                }
                _logger.Info($"Adding Crop for User: {user.UserId}");
                
                CropDTO newCrop = new CropDTO
                {
                    CropType = cropType,
                    Location = location,
                    PricePerUnit = pricePerUnit,
                    Quantity = quantity,
                    UserId = user.UserId
                };
                // var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                // var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                // // int userRole = int.Parse(roleClaim);
                // if (roleClaim != "Farmer") // Assuming 0 = Farmer, 1 = Dealer, 2 = Admin
                // {
                //     return Unauthorized("Only Farmers are allowed to add crops.");
                // }

                if (!await crop.AddCrop(newCrop))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Crop Added");
            }

            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateCrop")]
        [Authorize(Roles = "Admin,Farmer")]
        public async Task<IActionResult> UpdateCrop([FromBody] CropDTO u)
        {
            try
            {
                if (!await crop.UpdateCrop(u))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Crop Updated");
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("DeleteCrop/{cropId}")]
        [Authorize(Roles = "Admin,Farmer")]
        public async Task<IActionResult> DeleteCrop(string cropId)
        {
            try
            {
                if (!await crop.DeleteCrop(cropId))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Crop Deleted");
            }

            catch (Exception e)
            {
                _logger.Error($"Exception Occurred: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetCropsByUserId/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCropsByUserId(string userId)
        {
            try
            {
                var cropsForUser = await crop.GetCropsByUserId(userId);
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