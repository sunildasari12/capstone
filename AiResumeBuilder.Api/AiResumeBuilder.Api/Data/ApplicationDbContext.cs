using AiResumeBuilder.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AiResumeBuilder.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Table mapping
            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.Entity<Resume>().ToTable("Resume");
            modelBuilder.Entity<Education>().ToTable("Education");
            modelBuilder.Entity<Experience>().ToTable("Experience");

            // ✅ Relationships
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.Educations)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.Experiences)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Seed default Admin user
            var adminPassword = HashPassword("admin@123"); // change before production!
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    FullName = "System Admin",
                    Email = "admin@resumebuilder.com",
                    PasswordHash = adminPassword,
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );
        }

        // ✅ Helper to hash password
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

//using AiResumeBuilder.Api.Models;
//using Microsoft.EntityFrameworkCore;

//namespace AiResumeBuilder.Api.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options) { }

//        public DbSet<AppUser> Users { get; set; }
//        public DbSet<Resume> Resumes { get; set; }
//        public DbSet<Education> Educations { get; set; }
//        public DbSet<Experience> Experiences { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);


//            // Map AppUser to the actual SQL table
//            modelBuilder.Entity<AppUser>().ToTable("AppUser");

//            // Relationships
//            modelBuilder.Entity<Resume>()
//                .HasOne(r => r.User)
//                .WithMany(u => u.Resumes)
//                .HasForeignKey(r => r.UserId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Education>()
//                .HasOne(e => e.Resume)
//                .WithMany(r => r.Educations)
//                .HasForeignKey(e => e.ResumeId)
//                .OnDelete(DeleteBehavior.Cascade);

//            modelBuilder.Entity<Experience>()
//                .HasOne(e => e.Resume)
//                .WithMany(r => r.Experiences)
//                .HasForeignKey(e => e.ResumeId)
//                .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}
