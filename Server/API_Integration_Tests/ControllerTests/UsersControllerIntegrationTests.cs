using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API_Integration_Tests;

public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;

  public UsersControllerIntegrationTests(WebApplicationFactory<Program> factory)
  {
    client = factory.CreateClient();
  }

  private async Task AuthenticateAsync()
  {
    var loginDto = new UserLoginDto
    {
      Username = "newuser",
      Password = "password"
    };

    var loginContent = new StringContent(JsonSerializer.Serialize(loginDto), System.Text.Encoding.UTF8, "application/json");
    var loginResponse = await client.PostAsync("/Auth/login", loginContent);
    loginResponse.EnsureSuccessStatusCode();

    var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
    var userWithTokenDto = JsonSerializer.Deserialize<UserWithTokenDto>(loginResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userWithTokenDto.Token);
  }

  [Fact]
  public async Task GetUser_ShouldReturnOk()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/Users/1");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var user = JsonSerializer.Deserialize<UserReturnDto>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    Assert.NotNull(user);
  }

  [Fact]
  public async Task GetUser_ShouldReturnNotFound_WhenInvalidId()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/Users/9999");
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}