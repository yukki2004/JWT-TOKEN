using WebApplication3.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Domain.Entities;
namespace WebApplication3.Infrastructure.Data
{
    public class AppDataConText : DbContext
    {
        public AppDataConText(DbContextOptions<AppDataConText> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
    }
}
