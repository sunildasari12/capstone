using AiResumeBuilder.Api.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AiResumeBuilder.Api.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser> CreateAsync(AppUser user);
        Task<IEnumerable<AppUser>> GetAllAsync();
    }
}
