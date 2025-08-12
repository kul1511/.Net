using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;


    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> role, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = role;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.Email,
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpper(),
            NormalizedUserName = model.Email.ToUpper(),
            Role = model.Role
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
            return Ok(new { Message = "User Registered Successfully!" });

        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model, [FromQuery] string expectedRole)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return BadRequest(new
            {
                message = "Invalid Email",
                debug = $"User with email {model.Email} not found"
            });
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid)
            return BadRequest(new { message = "Invalid Password" });

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(expectedRole))
            return Unauthorized(new { message = $"Access Denied! You are not a {expectedRole}." });

        return Ok(new { message = "Login Successful!", user.FullName, user.Email, Role = roles.FirstOrDefault() ?? "No Role Assigned" });
    }


    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("FullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: System.DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
