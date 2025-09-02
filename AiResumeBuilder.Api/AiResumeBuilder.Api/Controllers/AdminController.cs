using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AiResumeBuilder.Api.Data;
using AiResumeBuilder.Api.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AiResumeBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _db.Users
                .Select(u => new { u.Id, u.FullName, u.Email, u.Role, u.CreatedAt })
                .ToList();

            return Ok(users);
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ✅ NEW: GET: api/admin/resumes
        [HttpGet("resumes")]
        public async Task<IActionResult> GetResumes()
        {
            var resumes = await _db.Resumes
                .Include(r => r.User) // If you want user info with resume
                .Select(r => new
                {
                    r.Id,
                    r.Title,
                    r.CreatedAt,
                    r.UpdatedAt,
                    User = new { r.User.Id, r.User.FullName, r.User.Email, r.User.Role }
                })
                .ToListAsync();

            return Ok(resumes);
        }
    }
}
