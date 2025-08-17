using WebApplication3.Domain.Entities;
namespace WebApplication3.Domain.Interface
{
    public interface IUserResposity
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserName(string userName);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);


    }
}
