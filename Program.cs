using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using WebApplication3.Domain.Interface;
using WebApplication3.Infrastructure.Data;
using WebApplication3.Infrastructure.Resposity;
using WebApplication3.Application.User.Queries;
using WebApplication3.Application.User.Command;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDataConText>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
#if !DEBUG
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
#endif
});

// HttpContextAccessor (cho Session)
builder.Services.AddHttpContextAccessor();

// GeminiService
builder.Services.AddSingleton<GeminiService>();

// MediatR (scan đúng assembly Application)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(WebApplication3.Application.User.Command.SendMassageCommandHandle).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(WebApplication3.Application.User.Queries.GetChatHistoriQueries).Assembly));

// Repository & Handlers
builder.Services.AddScoped<IUserResposity, UserResposity>();
builder.Services.AddScoped<LoginQueriesHandle>();
builder.Services.AddScoped<GetUserByIdQueriesHandle>();
builder.Services.AddScoped<DeleteUserCommandHandle>();
builder.Services.AddScoped<UpdateUserCommandHandle>();
builder.Services.AddScoped<GetUserQuerisHandle>();
builder.Services.AddScoped<AddUserCommandHandle>();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseAuthentication(); // phải có trước Authorization
app.UseAuthorization();
app.MapControllers();

app.Run();
