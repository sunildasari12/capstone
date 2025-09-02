using AiResumeBuilder.Api.Models;
using System.Security.Claims;

namespace AiResumeBuilder.Api.Services
{
    public interface IJwtService
    {
        //string GenerateToken(int userId, string email, string role);
        //ClaimsPrincipal? ValidateToken(string token);
        string GenerateToken(AppUser user);
    }
}

