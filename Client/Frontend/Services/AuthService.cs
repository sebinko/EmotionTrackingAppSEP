using System.Text;
using System.Text.Json;
using Entities;
using Frontend.Models;
using Frontend.Services.Interfaces;
using SharedUtil;

namespace Frontend.Services;

public class AuthService : IAuthService
{
  public async Task<AuthResponse?> Register(User user)
  {
    // TODO make the URL from config
    var url = "http://localhost:5195/Auth/register";
    var data = new
    {
      username = user.Username,
      password = user.Password,
      email = user.Email
    };
    using var client = new HttpClient();
    var json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
      var exStr = await response.Content.ReadAsStringAsync();
      var apiException = JsonSerializer.Deserialize<ApiExceptionResponse>(exStr,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });

      if (apiException is not null)
        throw new Exception($"{response.StatusCode}: {apiException.Error}");
      throw new Exception($"{response.StatusCode}: {exStr}");
    }

    var responseData = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var auth = JsonSerializer.Deserialize<AuthResponse>(responseData, options);

    return auth?.user is null ? null : auth;
  }

  public async Task<AuthResponse?> Login(string username, string password)
  {
    // TODO make the URL from config

    var url = "http://localhost:5195/Auth/login";
    var data = new
    {
      username, password
    };
    using var client = new HttpClient();
    var json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
      var exStr = await response.Content.ReadAsStringAsync();
      var apiException = JsonSerializer.Deserialize<ApiExceptionResponse>(exStr,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });

      if (apiException is not null)
        throw new Exception($"{response.StatusCode}: {apiException.Error}");
      throw new Exception($"{response.StatusCode}: {exStr}");
    }

    var responseData = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var auth = JsonSerializer.Deserialize<AuthResponse>(responseData, options);

    return auth?.user is null ? null : auth;
  }
}