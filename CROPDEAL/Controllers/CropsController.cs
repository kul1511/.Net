using CROPDEAL.Interfaces;
using CROPDEAL.Repository;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CropsController : ControllerBase
    {
        private readonly ILogger<CropsController> log;
        private readonly ICrops crop;
        public CropsController(ICrops _crop, ILogger<CropsController> llog)
        {
            crop = _crop;
            log = llog;
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpPost("AddCrop")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddCrop([FromBody] CropDTO u)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                    return Unauthorized("UserId not found in token.");

                int userId = int.Parse(userIdClaim.Value);

                u.UserId = userId;

                if (!await crop.AddCrop(u))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Crop Added\n Dealers has been Notified!!!");
            }

            catch (Exception e)
            {
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetCropsByUserId/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCropsByUserId(int userId)
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }
    }
}