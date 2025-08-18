using MediatR;
using WebApplication3.Application.User.DTOs;
using WebApplication3.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebApplication3.Application.User.Queries
{
    public class GetChatHistoryQuerisHandle : IRequestHandler<GetChatHistoriQueries, List<ChatMassageDTOs>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetChatHistoryQuerisHandle(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<List<ChatMassageDTOs>> Handle(GetChatHistoriQueries request, CancellationToken cancellationToken)
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var historyJson = session.GetString(request.SessionId);

            var history = string.IsNullOrEmpty(historyJson)
                ? new List<ChatMassage>()
                : JsonSerializer.Deserialize<List<ChatMassage>>(historyJson)!;

            var result = history.Select(h => new ChatMassageDTOs { Sender = h.Sender, Text = h.Text }).ToList();
            return Task.FromResult(result);
        }
    }

   
}
