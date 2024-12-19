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

public class EmotionsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;
  private readonly IUsersService usersService;
  private readonly AuthUtilities authUtilities;
  private string token;
  public EmotionsControllerIntegrationTests(WebApplicationFactory<Program> factory)
  {
    client = factory.CreateClient();
    var scope = factory.Services.CreateScope();
    usersService = scope.ServiceProvider.GetRequiredService<IUsersService>();
    authUtilities = scope.ServiceProvider.GetRequiredService<AuthUtilities>();
    GenerateToken();
  }

  private void GenerateToken()
  {
    var user = usersService.GetByUsername("newuser").Result;
    token = authUtilities.GenerateJwtToken(new UserReturnDto { Id = user.Id, Username = user.Username, Email = user.Email });
  }

  private async Task AuthenticateAsync()
  {
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
  
  [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.GetAsync("/Emotions");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var emotions = JsonSerializer.Deserialize<List<EmotionDto>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(emotions);
    }
}