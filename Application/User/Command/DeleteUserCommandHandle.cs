using WebApplication3.Domain.Interface; 
namespace WebApplication3.Application.User.Command
{
    public class DeleteUserCommandHandle
    {
        public readonly IUserResposity _userResposity;
        public DeleteUserCommandHandle(IUserResposity userResposity)
        {
            _userResposity = userResposity;
        }
        public async Task Handle(DeleteUserCommand deleteUserCommand)
        {
            var user = await _userResposity.GetUserByIdAsync(deleteUserCommand.id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            await _userResposity.DeleteUser(user.Id);
        }
    }
}
