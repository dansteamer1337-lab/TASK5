using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.Models;
using WebApplication7.Repositories.Interfaces;

namespace WebApplication7.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetDataAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetDataAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetDataAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"User with id {id} not found");
            }
        }

        public async Task EditAsync(User user)
        {
            var existingUser = await GetDataAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Login = user.Login;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"User with id {user.Id} not found");
            }
        }
    }
}