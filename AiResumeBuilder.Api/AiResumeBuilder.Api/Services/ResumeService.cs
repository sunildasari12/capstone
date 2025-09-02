using AiResumeBuilder.Api.Models;
using AiResumeBuilder.Api.Repositories;

namespace AiResumeBuilder.Api.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repo;
        public ResumeService(IResumeRepository repo) => _repo = repo;

        public Task<Resume> CreateAsync(Resume resume) => _repo.CreateAsync(resume);
        public Task<Resume?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Resume>> GetByUserIdAsync(int userId) => _repo.GetByUserIdAsync(userId);
        public Task UpdateAsync(Resume resume) => _repo.UpdateAsync(resume);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
