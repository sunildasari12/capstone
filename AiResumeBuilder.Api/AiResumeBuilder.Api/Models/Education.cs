using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AiResumeBuilder.Api.Models
{

    [Table("Education")]
    public class Education
    {
        public int Id { get; set; }
        [Required] 
        public int ResumeId { get; set; }
        [JsonIgnore]
        public Resume? Resume { get; set; }

        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
