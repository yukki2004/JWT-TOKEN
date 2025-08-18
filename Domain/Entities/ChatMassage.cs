namespace WebApplication3.Domain.Entities
{
    public class ChatMassage
    {
        public string Sender { get; set; } = string.Empty; // "user" hoặc "model"
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
