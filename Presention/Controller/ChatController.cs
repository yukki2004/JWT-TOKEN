using Microsoft.AspNetCore.Mvc;
using MediatR;
using WebApplication3.Application.User.Command;
using WebApplication3.Application.User.DTOs;
using WebApplication3.Application.User.Queries;
namespace WebApplication3.Presention.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMassageCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("history/{sessionId}")]
        public async Task<IActionResult> GetHistory(string sessionId)
        {
            var query = new GetChatHistoriQueries { SessionId = sessionId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
