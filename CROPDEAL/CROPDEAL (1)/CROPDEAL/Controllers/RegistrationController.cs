using CROPDEAL.Repository;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Services;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
// using CROPDEAL.Models.DTO;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        // private readonly RegisterRepository register;
        private readonly IAuth c;
        private readonly PasswordService ps;

        public RegistrationController(IAuth context, PasswordService passwordService)
        {
            c = context;
            ps = passwordService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User u)
        {
            if (await c.Register(u))
            {
                return Ok("User has been Registered Successfully");
            }
            return BadRequest();
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest u, [FromServices] TokenService tokenService)
        {
            if (u == null)
            {
                return BadRequest("Invalid login request!");
            }
            await c.LogToDatabase("Info", $"Login Attempt for Email: {u.Email} with Role: {u.Role}", DateTime.Now);

            // Validate Email and Password
            if (string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.Password_Hash))
            {
                await c.LogToDatabase("Failed", "Email or Password is missing", DateTime.Now);
                return BadRequest(new { message = "Email and Password are required!" });
            }

            // Check if user exists based on Email
            var user = await c.GetUserByEmailAsync(u.Email);
            if (user == null)
            {
                await c.LogToDatabase("Info", $"User not Found with Email: {u.Email}", DateTime.Now);
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            // Verify password using PasswordService
            if (!ps.VerifyPassword(user.Password_Hash, u.Password_Hash))
            {
                await c.LogToDatabase("Info", "Password Verification Failed", DateTime.Now);
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            if (!Enum.TryParse(typeof(UserRole), u.Role.ToString(), out var parsedRole) || (UserRole)parsedRole != user.Role)
            {
                await c.LogToDatabase("Failed", $"Role mismatch: Tried logging in with {u.Role}, but registered with {user.Role}", DateTime.Now);
                return Unauthorized(new { message = $"Invalid Role! Please login with the correct role: {user.Role}" });
            }

            // Generate JWT Token using user data
            var token = tokenService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                await c.LogToDatabase("Failed", "Token Generated Failed", DateTime.Now);
                return Unauthorized(new { message = "Token generation failed!" });
            }

            await c.LogToDatabase("Success", $"Token generated: {token}", DateTime.Now);

            // Return success response with token and role
            return Ok(new
            {
                message = "Logged in successfully!",
                token = token,
                role = user.Role
            });
        }
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await c.GetAllUsers();
                if (!users.Any())
                {
                    return NoContent();
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                await c.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User userDTO)
        {
            try
            {
                if (await c.UpdateUser(id, userDTO))
                {
                    return Ok("User profile updated successfully.");
                }
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                await c.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (await c.DeleteUser(id))
                {
                    return Ok("User profile deleted successfully.");
                }
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                await c.LogToDatabase("Error", $"Exception Occurred: {ex.Message}", DateTime.Now);
                return BadRequest(ex.Message);
            }
        }
    }
}