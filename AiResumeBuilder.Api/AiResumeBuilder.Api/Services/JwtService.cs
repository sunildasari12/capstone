//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace AiResumeBuilder.Api.Services
//{
//    public class JwtService : IJwtService
//    {
//        private readonly IConfiguration _config;
//        public JwtService(IConfiguration config) => _config = config;

//        public string GenerateToken(int userId, string email, string role)
//        {
//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, email),
//                new Claim(ClaimTypes.Role, role),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddHours(2),
//                signingCredentials: creds
//            );
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        public ClaimsPrincipal? ValidateToken(string token)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
//            try
//            {
//                var principal = handler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidIssuer = _config["Jwt:Issuer"],
//                    ValidateAudience = true,
//                    ValidAudience = _config["Jwt:Audience"],
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero
//                }, out _);
//                return principal;
//            }
//            catch
//            {
//                return null;
//            }
//        }
//    }
//}
using AiResumeBuilder.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AiResumeBuilder.Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
               

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

