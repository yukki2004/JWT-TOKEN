using WebApplication3.Domain.Interface;
namespace WebApplication3.Application.User.Command
{
    public class UpdateUserCommandHandle
    {
        public readonly IUserResposity _userResposity;
        public UpdateUserCommandHandle(IUserResposity userResposity)
        {
            _userResposity = userResposity;
        }
        public async Task Handle(UpdateUserCommand updateUserCommand)
        {
            var user = await _userResposity.GetUserByIdAsync(updateUserCommand.id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.FullName = updateUserCommand.fullName;
            user.Email = updateUserCommand.email;
            user.Address = updateUserCommand.address;
            user.Age = updateUserCommand.age;
            await _userResposity.UpdateUser(user);
        }
    }
}
