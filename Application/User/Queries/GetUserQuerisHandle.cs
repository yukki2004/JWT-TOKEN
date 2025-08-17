using WebApplication3.Domain.Interface;

namespace WebApplication3.Application.User.Queries
{
    public class GetUserQuerisHandle
    {
        public readonly IUserResposity _userResposity;
        public GetUserQuerisHandle(IUserResposity userResposity)
        {
            _userResposity = userResposity;
        }
        public async Task<List<DTOs.UserDTOs>> GetAllUsersAsync()
        {
            var users = await _userResposity.GetAllUsersAsync();
            return users.Select(user => new DTOs.UserDTOs
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email ?? string.Empty, 
                DiaChi = user.Address ?? string.Empty,
                Tuoi = user.Age
            }).ToList();
        }
    }
}
