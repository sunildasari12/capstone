namespace AiResumeBuilder.Api.DTOs
{
    public record RegisterDto(string FullName, string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record AuthResultDto(string Token, string Role, int UserId);
}
