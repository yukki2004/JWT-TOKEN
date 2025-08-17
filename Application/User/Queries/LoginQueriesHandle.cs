using WebApplication3.Domain.Interface;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication3.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace WebApplication3.Application.User.Queries
{
    public class LoginQueriesHandle
    {
        public readonly IUserResposity _userResposity;
        public readonly IConfiguration _cfg;
        public LoginQueriesHandle(IUserResposity userResposity, IConfiguration cfg)
        {
            _userResposity = userResposity;
            _cfg = cfg;

        }
        public async Task<String?> Handle(LoginQueries loginQueries)
        {
            var userName = await _userResposity.GetUserByUserName(loginQueries.username);
            if (userName == null) return null;
            if(userName != null && userName.PasswordHash != loginQueries.password) return null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userName.Id.ToString()),
                new Claim(ClaimTypes.Email, userName.Email),
                new Claim(ClaimTypes.Role, userName.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
