using Microsoft.AspNetCore.Mvc;
using AiResumeBuilder.Api.DTOs;
using AiResumeBuilder.Api.Models;
using AiResumeBuilder.Api.Repositories;
using AiResumeBuilder.Api.Services;
using BCrypt.Net; // Make sure BCrypt.Net-Next NuGet package is installed
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwt;

        public AuthController(IUserRepository userRepo, IJwtService jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var exists = await _userRepo.GetByEmailAsync(dto.Email);
            if (exists != null) return BadRequest("Email already registered");

            var user = new AppUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // ✅ use BCrypt
                Role = UserRole.User
            };

            var created = await _userRepo.CreateAsync(user);
            var token = _jwt.GenerateToken(created); // pass AppUser

            return Ok(new AuthResultDto(token, created.Role.ToString(), created.Id));
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user); // pass AppUser
            return Ok(new AuthResultDto(token, user.Role.ToString(), user.Id));
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using AiResumeBuilder.Api.DTOs;
//using AiResumeBuilder.Api.Models;
//using AiResumeBuilder.Api.Repositories;
//using AiResumeBuilder.Api.Services;
//using System.Security.Cryptography;
//using System.Text;
//using BCrypt.Net;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;

//namespace AiResumeBuilder.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]

//    public class AuthController : ControllerBase
//    {
//        private readonly IUserRepository _userRepo;
//        private readonly IJwtService _jwt;

//        public AuthController(IUserRepository userRepo, IJwtService jwt)
//        {
//            _userRepo = userRepo;
//            _jwt = jwt;
//        }

//        // POST: api/auth/register
//        [HttpPost("register")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Register(RegisterDto dto)
//        {
//            var exists = await _userRepo.GetByEmailAsync(dto.Email);
//            if (exists != null) return BadRequest("Email already registered");

//            var user = new AppUser
//            {
//                FullName = dto.FullName,
//                Email = dto.Email,
//                PasswordHash = HashPassword(dto.Password),
//                Role = UserRole.User
//            };

//            var created = await _userRepo.CreateAsync(user);
//            var token = _jwt.GenerateToken(created); // ✅ pass AppUser

//            return Ok(new AuthResultDto(token, created.Role.ToString(), created.Id));
//        }

//        // POST: api/auth/login
//        [HttpPost("login")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            var user = await _userRepo.GetByEmailAsync(dto.Email);
//            if (user == null) return Unauthorized("Invalid credentials");

//            //if (!VerifyPassword(dto.Password, user.PasswordHash))
//            //    return Unauthorized("Invalid credentials");
//            var inputHash = HashPassword(dto.Password);
//            Console.WriteLine($"DB Hash: {user.PasswordHash}");
//            Console.WriteLine($"Input Hash: {inputHash}");

//            if (inputHash != user.PasswordHash)
//                return Unauthorized("Invalid credentials");

//            var token = _jwt.GenerateToken(user); // ✅ pass AppUser
//            return Ok(new AuthResultDto(token, user.Role.ToString(), user.Id));
//        }

//        // Simple hashed password - replace with a proper password hasher in production
//        private string HashPassword(string password)
//        {
//            using var sha = SHA256.Create();
//            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
//            return Convert.ToBase64String(bytes);
//        }

//        private bool VerifyPassword(string password, string hash)
//        {
//            return HashPassword(password) == hash;
//        }
//    }
//}
