using WebApplication3.Application.User.Command;
using WebApplication3.Application.User.Queries;
using WebApplication3.Application.User.DTOs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
namespace WebApplication3.Presention.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly LoginQueriesHandle _loginQueriesHandle;
        public AuthController(LoginQueriesHandle loginQueriesHandle)
        {
            _loginQueriesHandle = loginQueriesHandle;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQueries loginQueries)
        {
            if (loginQueries == null || string.IsNullOrEmpty(loginQueries.username) || string.IsNullOrEmpty(loginQueries.password))
            {
                return BadRequest("Invalid login request.");
            }
            var token = await _loginQueriesHandle.Handle(loginQueries);
            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { Token = token });
        }

    }
}
