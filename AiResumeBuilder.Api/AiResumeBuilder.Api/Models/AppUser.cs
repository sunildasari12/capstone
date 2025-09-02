using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AiResumeBuilder.Api.Models
{
    public enum UserRole
    {
        Guest = 0,   // can view homepage/features only
        User = 1,    // can create, edit, download resumes
        Admin = 2    // manages users & system
    }

    public class AppUser
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Guest; // default role

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime? UpdatedAt { get; set; } // ✅ useful for tracking changes
        [NotMapped]
        public bool IsActive { get; set; } = true; // ✅ allow soft-delete/disable

        // Navigation property → One user can have many resumes
        [JsonIgnore]
        public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
    }
}


