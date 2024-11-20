using System.Text;
using System.Text.Json;
using API.DTO;
using Entities;
using Frontend.Services.Interfaces;
using SharedUtil;

namespace Frontend.Services;

public class AuthService : IAuthService
{
  public User user;
  private readonly IStorageService _storageService;
  private readonly HttpClient _httpClient;

  public AuthService(IStorageService storageService, HttpClient httpClient)
  {
    _storageService = storageService;
    _httpClient = httpClient;
  }

  public async Task<UserWithTokenDTO?> Register(User user)
  {
    var data = new
    {
      username = user.Username,
      password = user.Password,
      email = user.Email
    };

    var json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await _httpClient.PostAsync("Auth/register", content);

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

    var auth = JsonSerializer.Deserialize<UserWithTokenDTO>(responseData, options);
    await _storageService.SetItem("user", auth);
    return auth?.User is null ? null : auth;
  }

  public async Task<UserWithTokenDTO?> Login(string username, string password)
  {
    var data = new
    {
      username, password
    };

    var json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await _httpClient.PostAsync("Auth/login", content);

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

    var auth = JsonSerializer.Deserialize<UserWithTokenDTO>(responseData, options);
    await _storageService.SetItem("user", auth);

    return auth?.User is null ? null : auth;
  }

  public async Task<UserWithTokenDTO?> GetUser()
  {
    var json = await _storageService.GetItem<string>("user");
    Console.WriteLine("JSON from local storage: " + json);

    if (string.IsNullOrEmpty(json))
    {
      return null;
    }

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    return JsonSerializer.Deserialize<UserWithTokenDTO>(json, options);
  }

  public async Task Logout()
  {
    await _storageService.RemoveItem("user");
  }
}