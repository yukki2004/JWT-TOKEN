using WebApplication3.Application.User.DTOs;
using WebApplication3.Domain.Entities;
using WebApplication3.Infrastructure.Resposity;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using MediatR;

namespace WebApplication3.Application.User.Command
{
    public class SendMassageCommandHandle : IRequestHandler<SendMassageCommand, ChatMassageDTOs>
    {
        private readonly GeminiService _gemini;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendMassageCommandHandle(GeminiService gemini, IHttpContextAccessor httpContextAccessor)
        {
            _gemini = gemini;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChatMassageDTOs> Handle(SendMassageCommand request, CancellationToken cancellationToken)
        {
            var session = _httpContextAccessor.HttpContext!.Session;

            // Nếu client không truyền SessionId thì backend tự tạo
            var sessionId = string.IsNullOrWhiteSpace(request.SessionId)
                ? Guid.NewGuid().ToString()
                : request.SessionId;

            // Lấy lịch sử từ session
            var historyJson = session.GetString(sessionId);
            var history = string.IsNullOrEmpty(historyJson)
                ? new List<ChatMassage>()
                : JsonSerializer.Deserialize<List<ChatMassage>>(historyJson)!;

            // Thêm tin nhắn user
            history.Add(new ChatMassage { Sender = "user", Text = request.UserMessage });

            // Gọi Gemini API
            var reply = await _gemini.GenerateContentAsync(request.UserMessage);

            // Thêm tin nhắn model
            history.Add(new ChatMassage { Sender = "model", Text = reply });

            // Lưu lại vào session
            session.SetString(sessionId, JsonSerializer.Serialize(history));

            // Trả về kèm SessionId để client lưu lại
            return new ChatMassageDTOs
            {
                SessionId = sessionId,
                Sender = "model",
                Text = reply
            };
        }
    }
}
