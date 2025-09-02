using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AiResumeBuilder.Api.Models
{
    [Table("Resume")]
    public class Resume
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        // ✅ must be User, not UserRole
        [JsonIgnore]
        public AppUser? User { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
