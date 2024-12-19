using System.Text;
using API.Auth;
using API.Middlewares;
using API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json")
  .Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IEmotionCheckInService, EmotionCheckInService>();
builder.Services.AddScoped<IEmotionsService, EmotionsService>();
builder.Services.AddScoped<IUserTagsService, UserTagsService>();
builder.Services.AddScoped<IUserFriendsService, UserFriendsService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddSingleton<AuthUtilities>();

var secretKey = configuration["ApplicationSettings:SecretKey"];

builder.Services.AddSingleton(new PasswordHasherUtil(secretKey!));

builder.Services.AddAuthentication(cfg =>
{
  cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
  x.RequireHttpsMetadata = false;
  x.SaveToken = false;
  x.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
      Encoding.UTF8
        .GetBytes(configuration["ApplicationSettings:JWT_Secret"])
    ),
    ValidateIssuer = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero
  };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program
{
}