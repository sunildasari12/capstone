using AiResumeBuilder.Api.Models;

namespace ResumeBuilder.Api.DTOs
{
    public record ResumeCreateDto(int UserId, string Title, string Summary);
    public record ResumeUpdateDto(string Title, string Summary);
}
