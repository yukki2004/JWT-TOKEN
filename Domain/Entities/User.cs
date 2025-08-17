namespace WebApplication3.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = null!;
    }
}
