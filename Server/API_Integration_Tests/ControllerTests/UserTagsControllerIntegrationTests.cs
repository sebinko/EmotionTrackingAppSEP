using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API_Integration_Tests;

public class UserTagsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;

  public UserTagsControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
  public async Task GetAllTags_ShouldReturnOk()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/UserTags");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var tags = JsonSerializer.Deserialize<List<TagDto>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    Assert.NotNull(tags);
  }

  [Fact]
  public async Task GetAllTags_ShouldReturnUnauthorized_WhenNotAuthenticated()
  {
    var response = await client.GetAsync("/UserTags");

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}