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
builder.Services.AddScoped<IStorageService, SessionStorageService>();
builder.Services.AddScoped<IEmotionsService, EmotionsService>();
builder.Services.AddScoped<IEmotionCheckInService, EmotionCheckInService>();

builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddScoped<AuthedClient>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5195") });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", true);
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
  .AddInteractiveServerRenderMode();

app.Run();