using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API_Integration_Tests;

public class ReactionsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;

  public ReactionsControllerIntegrationTests(WebApplicationFactory<Program> factory)
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

    var loginContent = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
    var loginResponse = await client.PostAsync("/Auth/login", loginContent);
    loginResponse.EnsureSuccessStatusCode();

    var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
    var userWithTokenDto = JsonSerializer.Deserialize<UserWithTokenDto>(loginResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userWithTokenDto.Token);
  }
  
  [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await AuthenticateAsync();

        var createDto = new ReactionCreateDto
        {
          Emoji = "😊",
          EmotionCheckInId = 1
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/Reactions", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenInvalidId()
    {
        await AuthenticateAsync();

        var createDto = new ReactionCreateDto
        {
          Emoji = "😊",
          EmotionCheckInId = 99999 //invalid Id
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/Reactions", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.DeleteAsync("/Reactions/1");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var createDto = new ReactionCreateDto
        {
          Emoji = "😊",
          EmotionCheckInId = 1
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/Reactions", content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var response = await client.DeleteAsync("/Reactions/1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}