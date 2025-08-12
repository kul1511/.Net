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
        private readonly ICrops crop;
        public CropsController(ICrops _crop)
        {
            crop = _crop;
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddCrop")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddCrop([FromBody] CropDTO u)
        {
            try
            {
                // var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                // var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                // // int userRole = int.Parse(roleClaim);
                // if (roleClaim != "Farmer") // Assuming 0 = Farmer, 1 = Dealer, 2 = Admin
                // {
                //     return Unauthorized("Only Farmers are allowed to add crops.");
                // }

                if (!await crop.AddCrop(u))
                {
                    return BadRequest("Wrong Request");
                }
                return Ok("Crop Added");
            }

            catch (Exception e)
            {
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
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
                await crop.LogToDatabase("Error", $"Exception Occurred: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
    }
}