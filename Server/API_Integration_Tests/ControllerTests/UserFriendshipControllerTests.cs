using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Auth;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Protobuf.Services.Interfaces;

namespace API_Integration_Tests;

public class UserFriendshipControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;
  private readonly IUsersService usersService;
  private readonly AuthUtilities authUtilities;
  private string token;

    public UserFriendshipControllerTests(WebApplicationFactory<Program> factory)
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
    public async Task GetAllFriends_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.GetAsync("/UserFriendship");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var friendships = JsonSerializer.Deserialize<List<CreateFriendshipDto>>(responseString, new JsonSerializerOptions 
        { 
          PropertyNameCaseInsensitive = true,
          DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        Assert.NotNull(friendships);
        Assert.All(friendships, friendship => Assert.NotNull(friendship.user2UserName));
    }

    [Fact]
    public async Task CreateFriendship_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var createDto = new CreateFriendshipDto
        {
            user2UserName = "jake_peralta"
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/UserFriendship", content);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateFriendship_ShouldReturnNotFound_WhenInvalidUsername()
    {
        await AuthenticateAsync();

        var createDto = new CreateFriendshipDto
        {
            user2UserName = "invaliduser"
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/UserFriendship", content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveFriendship_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.DeleteAsync("/UserFriendship/jake_peralta");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task RemoveFriendship_ShouldReturnNotFound_WhenInvalidUsername()
    {
        await AuthenticateAsync();

        var response = await client.DeleteAsync("/UserFriendship/invaliduser");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
  
}