using CROPDEAL.Repository;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Services;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using log4net;
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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RegistrationController));

        // private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IAuth context, PasswordService passwordService)
        {
            c = context;
            ps = passwordService;
            // _logger = logger;
        }


        // [HttpGet("Authorization")]
        // [Authorize(Roles = "Admin,Farmer,Dealer")]
        // public async Task<IActionResult> Authorization()
        // {

        //     return Ok( await c.CheckRole());
            
        // }

        ///<remarks>
        /// (Note: For Role, Select 0: Farmer, 1: Dealer, 2: Admin)
        ///</remarks>

        [HttpPost("register")]
        public async Task<IActionResult> Register(string FullName, string Email, string Password, UserRole userRole)
        {
            User newUser = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FullName = FullName,
                Email = Email,
                Password_Hash = Password,
                Role = userRole
            };

            if (await c.Register(newUser))
            {
                _logger.Info($"User Registered Successfully with Email: {Email} and Role: {newUser.Role}");
                return Ok("User has been Registered Successfully");
            }
            _logger.Warn($"User registration failed for Email: {Email}");
            return BadRequest();
        }


        ///<remarks>
        /// (Note: For Role, Select 0: Farmer, 1: Dealer, 2: Admin)
        ///</remarks>
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password, UserRole role, [FromServices] TokenService tokenService)
        {
            LoginRequest u = new LoginRequest
            {
                Email = email,
                Password_Hash = password,
                Role = role
            };

            if (u == null)
            {
                _logger.Warn("Invalid login request!");
                return BadRequest("Invalid login request!");
            }
            _logger.Info($"Login Attempt for Email: {u.Email} with Role: {u.Role}");

            // Validate Email and Password
            if (string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.Password_Hash))
            {
                _logger.Warn($"Login Failed for Email: {u.Email} - Reason: Email or Password is missing");
                return BadRequest(new { message = "Email and Password are required!" });
            }

            // Check if user exists based on Email
            var user = await c.GetUserByEmailAsync(u.Email);
            if (user == null)
            {
                _logger.Info($"User not Found with Email: {u.Email}");
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            // Verify password using PasswordService
            if (!ps.VerifyPassword(user.Password_Hash!, u.Password_Hash))
            {
                _logger.Info("Password Verification Failed");
                return Unauthorized(new { message = "Invalid Email or Password!" });
            }

            if (!Enum.TryParse(typeof(UserRole), u.Role.ToString(), out var parsedRole) || (UserRole)parsedRole != user.Role)
            {
                _logger.Error($"Role mismatch: Tried logging in with {u.Role}, but registered with {user.Role}");
                return Unauthorized(new { message = $"Invalid Role! Please login with the correct role: {user.Role}" });
            }

            // Generate JWT Token using user data
            var token = tokenService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                _logger.Error("Token generation failed!");
                return Unauthorized(new { message = "Token generation failed!" });
            }

            _logger.Info($"Token generated for Email: {u.Email}");

            // Return success response with token and user data
            return Ok(new
            {
                token = token,
                user = new
                {
                    userId = user.UserId,
                    fullName = user.FullName,
                    email = user.Email,
                    role = user.Role
                }
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
                    _logger.Info("No users found.");
                    return NoContent();
                }
                _logger.Info($"Fetched {users.Count()} users.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
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
                    _logger.Info($"User profile updated successfully for UserId: {id}");
                    return Ok("User profile updated successfully.");
                }
                _logger.Warn($"User not found for update. UserId: {id}");
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
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
                    _logger.Info($"User profile deleted successfully for UserId: {id}");
                    return Ok("User profile deleted successfully.");
                }
                _logger.Warn($"User not found for deletion. UserId: {id}");
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}