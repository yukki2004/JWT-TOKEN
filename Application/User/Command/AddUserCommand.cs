namespace WebApplication3.Application.User.Command
{
    public class AddUserCommand
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string fullName { get; set; } = null!;
        public int age { get; set; }
        public string? address { get; set; } = null!;
        public string? email { get; set; } = null!;
        public string role { get; set; } = "User";

    }
}
