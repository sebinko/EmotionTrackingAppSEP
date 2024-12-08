using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using API.DTO;
using Entities;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using SharedUtil;

namespace Frontend.Services;

public class AuthService : AuthenticationStateProvider
{
  private readonly IStorageService _storageService;
  private readonly HttpClient _httpClient;
  private ClaimsPrincipal currentClaimsPrincipal;

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

    List<Claim> claims = new()
    {
      new Claim(ClaimTypes.Name, auth?.User?.Username),
      new Claim(ClaimTypes.Email, auth?.User?.Email),
      new Claim(ClaimTypes.NameIdentifier, auth?.User?.Id.ToString()),
      new Claim("Token", auth?.Token.ToString()),
      new Claim("Streak", auth.User.Streak?.ToString())
    };

    ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
    currentClaimsPrincipal = new ClaimsPrincipal(identity);


    NotifyAuthenticationStateChanged(
      Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));

    await _storageService.SetItem("auth", auth);

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

    List<Claim> claims = new()
    {
      new Claim(ClaimTypes.Name, auth?.User?.Username),
      new Claim(ClaimTypes.Email, auth?.User?.Email),
      new Claim(ClaimTypes.NameIdentifier, auth?.User?.Id.ToString()),
      new Claim("Token", auth?.Token.ToString()),
      new Claim("Streak", auth.User.Streak?.ToString())
    };

    ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
    currentClaimsPrincipal = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(
      Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));

    await _storageService.SetItem("auth", auth);

    return auth?.User is null ? null : auth;
  }

  public async Task<UserReturnDTO> ChangePassword(UserWithTokenDTO userWithTokenDto,
    string newPassword)
  {
    var data = new ChangePasswordDTO
    {
      NewPassword = newPassword
    };

    var json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // add jwt token to the request
    _httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", userWithTokenDto.Token);

    var response = await _httpClient.PatchAsync("Auth/change-password", content);

    if (!response.IsSuccessStatusCode)
    {
      var exStr = await response.Content.ReadAsStringAsync();
      Console.WriteLine(response.StatusCode);
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

    var user = JsonSerializer.Deserialize<UserReturnDTO>(responseData, options);

    return user;
  }

  public async Task<UserWithTokenDTO> GetAuth()
  {
    return await _storageService.GetItem<UserWithTokenDTO>("auth");
  }

  public async Task Logout()
  {
    await _storageService.RemoveItem("auth");
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var auth = await _storageService.GetItem<UserWithTokenDTO>("auth");

    if (auth is null)
    {
      return new AuthenticationState(new ClaimsPrincipal());
    }

    List<Claim> claims = new()
    {
      new Claim(ClaimTypes.Name, auth.User.Username),
      new Claim(ClaimTypes.Email, auth.User.Email),
      new Claim(ClaimTypes.NameIdentifier, auth.User.Id.ToString()),
      new Claim("Token", auth.Token.ToString())
    };

    ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
    currentClaimsPrincipal = new ClaimsPrincipal(identity);

    return new AuthenticationState(currentClaimsPrincipal);
  }
}