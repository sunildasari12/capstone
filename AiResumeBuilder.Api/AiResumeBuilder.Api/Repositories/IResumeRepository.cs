using AiResumeBuilder.Api.Models;

namespace AiResumeBuilder.Api.Repositories
{
    public interface IResumeRepository
    {
        Task<Resume> CreateAsync(Resume resume);
        Task<Resume?> GetByIdAsync(int id);
        Task<IEnumerable<Resume>> GetByUserIdAsync(int userId);
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(int id);
    }
}
