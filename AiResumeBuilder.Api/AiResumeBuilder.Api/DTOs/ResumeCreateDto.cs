namespace AiResumeBuilder.Api.DTOs
{
    public class ResumeCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}
