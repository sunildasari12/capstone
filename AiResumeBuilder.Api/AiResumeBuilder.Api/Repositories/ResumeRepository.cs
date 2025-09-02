using Microsoft.EntityFrameworkCore;
using AiResumeBuilder.Api.Data;
using AiResumeBuilder.Api.Models;

namespace AiResumeBuilder.Api.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _db;
        public ResumeRepository(ApplicationDbContext db) => _db = db;

        public async Task<Resume> CreateAsync(Resume resume)
        {
            _db.Resumes.Add(resume);
            await _db.SaveChangesAsync();
            return resume;
        }

        public async Task<Resume?> GetByIdAsync(int id) =>
            await _db.Resumes
                .Include(r => r.Educations)
                .Include(r => r.Experiences)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Resume>> GetByUserIdAsync(int userId) =>
            await _db.Resumes.Where(r => r.UserId == userId).ToListAsync();

        public async Task UpdateAsync(Resume resume)
        {
            _db.Resumes.Update(resume);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _db.Resumes.FindAsync(id);
            if (r != null) { _db.Resumes.Remove(r); await _db.SaveChangesAsync(); }
        }
    }
}
