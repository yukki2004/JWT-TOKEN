
using Microsoft.EntityFrameworkCore;
using WebApplication3.Infrastructure.Data;
using WebApplication3.Domain.Interface;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication3.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication3
{
    public class Program
    {
   

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDataConText>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });
            builder.Services.AddScoped<IUserResposity, Infrastructure.Resposity.UserResposity>();
            builder.Services.AddScoped<Application.User.Queries.LoginQueriesHandle>();
            builder.Services.AddScoped<Application.User.Queries.GetUserByIdQueriesHandle>();
            builder.Services.AddScoped<Application.User.Command.DeleteUserCommandHandle>();
            builder.Services.AddScoped<Application.User.Command.UpdateUserCommandHandle>();
            builder.Services.AddScoped<Application.User.Queries.GetUserQuerisHandle>();
            builder.Services.AddScoped<Application.User.Command.AddUserCommandHandle>();

            builder.Services.AddControllers();
         
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
