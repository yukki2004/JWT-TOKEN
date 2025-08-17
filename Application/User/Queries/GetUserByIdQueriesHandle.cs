using WebApplication3.Domain.Interface;
namespace WebApplication3.Application.User.Queries
{
    public class GetUserByIdQueriesHandle
    {
        public readonly IUserResposity _userResposity;
        public GetUserByIdQueriesHandle(IUserResposity userResposity)
        {
            _userResposity = userResposity;
        }
        public async Task<DTOs.UserDTOs> GetUserByIdAsync(GetUserByIdQueries getUserByIdQueries)
        {
            var user = await _userResposity.GetUserByIdAsync(getUserByIdQueries.id);
            if (user == null)
            {
                return null;
            }
            return new DTOs.UserDTOs
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email ?? string.Empty, 
                DiaChi = user.Address ?? string.Empty, 
                Tuoi = user.Age
            };
        }

    }
}
