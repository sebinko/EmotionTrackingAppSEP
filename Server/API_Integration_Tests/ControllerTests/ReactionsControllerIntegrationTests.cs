using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.Auth;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Protobuf.Services.Interfaces;

namespace API_Integration_Tests;

public class ReactionsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;
  private readonly IUsersService usersService;
  private readonly AuthUtilities authUtilities;
  private string token;

  public ReactionsControllerIntegrationTests(WebApplicationFactory<Program> factory)
  {
    client = factory.CreateClient();
    var scope = factory.Services.CreateScope();
    usersService = scope.ServiceProvider.GetRequiredService<IUsersService>();
    authUtilities = scope.ServiceProvider.GetRequiredService<AuthUtilities>();
    GenerateToken();
  }

  private void GenerateToken()
  {
    var user = usersService.GetByUsername("jake_peralta").Result;
    token = authUtilities.GenerateJwtToken(new UserReturnDto { Id = user.Id, Username = user.Username, Email = user.Email });
  }

  private async Task AuthenticateAsync()
  {
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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