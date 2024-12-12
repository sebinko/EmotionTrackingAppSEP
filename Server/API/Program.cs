using System.Text;
using API.Auth;
using API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Protobuf.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json")
  .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<EmotionCheckInService>();
builder.Services.AddScoped<EmotionsService>();
builder.Services.AddScoped<UserTagsService>();
builder.Services.AddScoped<UserFriendsService>();
builder.Services.AddSingleton<AuthUtilities>();
builder.Services.AddScoped<ReactionService>();

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