using WebApplication3.Domain.Interface;
namespace WebApplication3.Application.User.Command
{
    public class AddUserCommandHandle
    {
        public readonly IUserResposity _userResposity;
        public AddUserCommandHandle(IUserResposity userResposity)
        {
            _userResposity = userResposity;
        }
        public async Task Handle(AddUserCommand addUserCommand)
        {
            var user = new Domain.Entities.User
            {
                FullName = addUserCommand.fullName,
                Email = addUserCommand.email,
                Address = addUserCommand.address,
                Age = addUserCommand.age,
                UserName = addUserCommand.username,
                PasswordHash = addUserCommand.password,
                Role = addUserCommand.role

            };
            await _userResposity.AddUser(user);
        }

    }
}
