using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AiResumeBuilder.Api.Models
{
    [Table("Experience")]
    public class Experience
    {
        public int Id { get; set; }
        [Required]
        public int ResumeId { get; set; }
        [JsonIgnore]
        public Resume? Resume { get; set; }

        public string Company { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; } // null = present
    }
}
