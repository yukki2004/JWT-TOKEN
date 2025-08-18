using MediatR;
using WebApplication3.Application.User.DTOs;

namespace WebApplication3.Application.User.Queries
{
    public class GetChatHistoriQueries : IRequest<List<ChatMassageDTOs>>
    {
        public string SessionId { get; set; } = string.Empty;
    }
}
