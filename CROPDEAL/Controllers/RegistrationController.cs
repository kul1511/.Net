using CROPDEAL.Repository;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Services;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Stripe.Tax;
// using CROPDEAL.Models.DTO;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> log;
        private readonly IAuth c;
        private readonly PasswordService ps;

        public RegistrationController(IAuth context, PasswordService passwordService, ILogger<RegistrationController> _logger)
        {
            c = context;
            ps = passwordService;
            log = _logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User u)
        {
            try
            {
                if (await c.Register(u))
                {
                    return Ok("User has been Registered Successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                // log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest u, [FromServices] TokenService tokenService)
        {
            if (u == null)
            {
                return BadRequest("Invalid login request!");
            }
            log.LogInformation($"Login Attempt for Email: {u.Email}", DateTime.Now);

            // Validate Email and Password
            if (string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.Password_Hash))
            {
                log.LogError("Email or Password is missing in Request", DateTime.Now);
                return BadRequest(new { message = "Email and Password are required!" });
            }

            // Check if user exists based on Email
            var user = await c.GetUserByEmailAsync(u.Email);
            if (user == null)
            {
                log.LogInformation($"User not Found with Email: {u.Email}", DateTime.Now);
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            // Verify password using PasswordService
            if (!ps.VerifyPassword(user.Password_Hash, u.Password_Hash))
            {
                log.LogError("Password Verification Failed", DateTime.Now);
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            // if (!Enum.TryParse(typeof(UserRole), u.Role.ToString(), out var parsedRole) || (UserRole)parsedRole != user.Role)
            // {
            //     log.LogError($"Role mismatch: Tried logging in with {u.Role}, but registered with {user.Role}", DateTime.Now);
            //     return Unauthorized(new { message = $"Invalid Role! Please login with the correct role: {user.Role}" });
            // }

            // Generate JWT Token using user data
            var token = tokenService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                log.LogError("Token Generated Failed", DateTime.Now);
                return Unauthorized(new { message = "Token generation failed!" });
            }

            log.LogInformation($"Token generated: {token}", DateTime.Now);

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
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpPut("UpdateUser/{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] User userDTO)
        {
            try
            {
                if (await c.UpdateUser(email, userDTO))
                {
                    return Ok("User profile updated successfully.");
                }
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
        [HttpDelete("DeleteUser/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                if (await c.DeleteUser(email))
                {
                    return Ok("User profile deleted successfully.");
                }
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
    }
}