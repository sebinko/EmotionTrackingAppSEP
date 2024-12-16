using ApexCharts;
using Blazored.SessionStorage;
using Frontend.Components;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
string startUrl = builder.Configuration[WebHostDefaults.ServerUrlsKey];


// Add services to the container.
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddApexCharts();


builder.Services.AddScoped<IStorageService, SessionStorageService>();
builder.Services.AddScoped<IEmotionsService, EmotionsService>();
builder.Services.AddScoped<IEmotionCheckInService, EmotionCheckInService>();
builder.Services.AddScoped<IUserTagsService, UserTagsService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.AddScoped<IUserFriendsService, UserFriendsService>();
var baseUrl = startUrl == "https://localhost:5216" ? "https://localhost:5195" : "http://localhost:5195";
builder.Services.AddScoped(sp => new AuthedClient(new HttpClient(), sp.GetRequiredService<AuthenticationStateProvider>(), baseUrl));
builder.Services.AddScoped(sp => new NonAuthedClient(new HttpClient(), baseUrl));
// add http client



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