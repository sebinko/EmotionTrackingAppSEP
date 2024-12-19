using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API_Integration_Tests;

public class UserFriendshipControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;

    public UserFriendshipControllerTests(WebApplicationFactory<Program> factory)
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
    public async Task GetAllFriends_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.GetAsync("/UserFriendship");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var friendships = JsonSerializer.Deserialize<List<CreateFriendshipDto>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(friendships);
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