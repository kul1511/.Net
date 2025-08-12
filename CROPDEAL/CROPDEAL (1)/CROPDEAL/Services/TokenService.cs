using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using CROPDEAL.Models;

namespace CROPDEAL.Services
{
    public class TokenService
    {
        private readonly IConfiguration config;
        public TokenService(IConfiguration _config) => config = _config;


        public string GenerateToken(User u)
        {
            var key = config["JwtSettings:SecretKey"];

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("JwtSettings:SecretKey is missing in appsettings.json");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name,u.Email),
                new Claim(ClaimTypes.Role,u.Role.ToString())
            };

            var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["JwtSettings:ExpirationMinutes"])),
            signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}