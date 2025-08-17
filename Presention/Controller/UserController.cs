using WebApplication3.Application.User.Command;
using WebApplication3.Application.User.Queries;
using WebApplication3.Application.User.DTOs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace WebApplication3.Presention.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly AddUserCommandHandle _addUserCommand;
        public readonly GetUserByIdQueriesHandle _getUserByIdQueriesHandle;
        public readonly GetUserQuerisHandle _getUserQuerisHandle;
        public readonly DeleteUserCommandHandle _deleteUserCommandHandle;
        public readonly UpdateUserCommandHandle _updateUserCommandHandle;
        public UserController(AddUserCommandHandle addUserCommand, GetUserByIdQueriesHandle getUserByIdQueriesHandle, GetUserQuerisHandle getUserQuerisHandle, DeleteUserCommandHandle deleteUserCommandHandle, UpdateUserCommandHandle updateUserCommandHandle)
        {
            _addUserCommand = addUserCommand;
            _getUserByIdQueriesHandle = getUserByIdQueriesHandle;
            _getUserQuerisHandle = getUserQuerisHandle;
            _deleteUserCommandHandle = deleteUserCommandHandle;
            _updateUserCommandHandle = updateUserCommandHandle;
        }
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand addUserCommand)
        {
            if (addUserCommand == null || string.IsNullOrEmpty(addUserCommand.fullName) || string.IsNullOrEmpty(addUserCommand.email))
            {
                return BadRequest("Invalid user data.");
            }
            await _addUserCommand.Handle(addUserCommand);
            return Ok("User added successfully.");
        }
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var command = new DeleteUserCommand { id = id };

            if (command.id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            await _deleteUserCommandHandle.Handle(command);
            return Ok("User deleted successfully.");
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _getUserQuerisHandle.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }
        [HttpGet("get/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var command = new GetUserByIdQueries { id = id };
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if(currentRole == "User" && user != command.id.ToString())
            {
                return Unauthorized("You do not have permission to access this user.");
            }
            var result = await _getUserByIdQueriesHandle.GetUserByIdAsync(command);
            if (result == null)
            {
                return NotFound("User not found.");
            }
            return Ok(result);

        }
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand updateUserCommand)
        {
            if (updateUserCommand == null || updateUserCommand.id <= 0 || string.IsNullOrEmpty(updateUserCommand.fullName) || string.IsNullOrEmpty(updateUserCommand.email))
            {
                return BadRequest("Invalid user data.");
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (currentRole == "User" && user != updateUserCommand.id.ToString())
            {
                return Unauthorized("You do not have permission to access this user.");
            }
            await _updateUserCommandHandle.Handle(updateUserCommand);
            return Ok("User updated successfully.");
        }

    }
}
