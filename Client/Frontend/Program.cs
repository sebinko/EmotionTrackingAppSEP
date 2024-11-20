using Blazored.SessionStorage;
using Frontend.Components;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddHttpClient<IStatusService, StatusService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStorageService, SessionStorageService>();
builder.Services.AddScoped<IEmotionsService, EmotionsService>();
builder.Services.AddScoped<IEmotionCheckInService, EmotionCheckInService>();

builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5195") });
builder.Services.AddHttpClient("AuthClient")
  .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5195"))
  .AddHttpMessageHandler(sp =>
  {
    var authService = sp.GetRequiredService<IAuthService>();
    return new AuthTokenHandler(authService);
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

app.Run();