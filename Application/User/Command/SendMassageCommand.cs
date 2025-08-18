using MediatR;
using WebApplication3.Application.User.DTOs;

namespace WebApplication3.Application.User.Command
{
    public class SendMassageCommand : IRequest<ChatMassageDTOs>
    {
        public string UserMessage { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
    }
}
