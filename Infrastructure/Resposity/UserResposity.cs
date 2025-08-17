using WebApplication3.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Infrastructure.Data;
namespace WebApplication3.Infrastructure.Resposity
{
    public class UserResposity : IUserResposity
    {
        public readonly AppDataConText _context;
        public UserResposity(AppDataConText context)
        {
            _context = context;
        }
        public async Task<List<Domain.Entities.User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<Domain.Entities.User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUser(Domain.Entities.User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(Domain.Entities.User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Domain.Entities.User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
