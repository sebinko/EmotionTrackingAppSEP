using System.Text;
using System.Text.Json;
using Frontend.Data;
using Frontend.Models;
using Frontend.Services.Interfaces;

namespace Frontend.Services;

public class UsersService : IUsersService
{
  public async Task<User> Create(User user)
  {
    var url = "http://localhost:5195/Auth/register";
    var data = new
    {
      username = user.Username,
      password = user.Password,
      email = user.Email,
    };
    using var client = new HttpClient();
    var json = JsonSerializer.Serialize(data);
    Console.WriteLine(json);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
      Console.WriteLine("Error: " + response.StatusCode);
    }
      
    var responseData = await response.Content.ReadAsStringAsync();
                
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    
    return JsonSerializer.Deserialize<User>(responseData, options);
  }

  public async Task<AuthResponse?> GetByUsernameAndPassword(string username, string password)
  {
    var url = "http://localhost:5195/Auth/login";
    var data = new
    {
      username = username,
      password = password
    };
    using var client = new HttpClient();
    var json = JsonSerializer.Serialize(data);
    Console.WriteLine(json);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
      Console.WriteLine("Error: " + response.Content.ReadAsStringAsync().Result);
    }
      
    var responseData = await response.Content.ReadAsStringAsync();
                
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    
    return JsonSerializer.Deserialize<AuthResponse>(responseData, options);
  }
}