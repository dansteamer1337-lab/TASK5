using WebApplication7.Models;

namespace WebApplication7.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetDataAsync();
        Task<User?> GetDataAsync(int id);
        Task AddAsync(User user);
        Task DeleteAsync(int id);
        Task EditAsync(User user);
    }
}