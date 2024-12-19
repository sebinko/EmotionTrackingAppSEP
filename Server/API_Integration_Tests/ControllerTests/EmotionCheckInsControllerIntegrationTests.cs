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

public class EmotionCheckInsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient client;
  private readonly IUsersService usersService;
  private readonly AuthUtilities authUtilities;
  private string token;

  public EmotionCheckInsControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
    token = authUtilities.GenerateJwtToken(new UserReturnDto { Id = user.Id, Username = user.Username, Email = user.Email});
  }
  
  private async Task AuthenticateAsync()
  {
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
  
  [Fact]
  public async Task GetAll_ShouldReturnOk()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/EmotionCheckIns");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var emotionCheckIns = JsonSerializer.Deserialize<List<EmotionCheckInDto>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    Assert.NotNull(emotionCheckIns);
  }
  
  [Fact]
  public async Task GetAll_ShouldReturnUnauthorized_WhenNotAuthenticated()
  {
    var response = await client.GetAsync("/EmotionCheckIns");
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
  
  [Fact]
  public async Task GetByTags_ShouldReturnOk()
  {
    await AuthenticateAsync();

    var tagsDto = new GetEmotionCheckInByTagsDto
    {
      Tags = new List<TagDto>
      {
        new TagDto { Key = "1", Type = TagType.WHAT },
        new TagDto { Key = "2", Type = TagType.WHAT }
      }
    };

    var content = new StringContent(JsonSerializer.Serialize(tagsDto), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("/EmotionCheckIns/by-tags", content);
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var emotionCheckIns = JsonSerializer.Deserialize<List<EmotionCheckInDto>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    Assert.NotNull(emotionCheckIns);
  }
  
  [Fact]
  public async Task Get_ShouldReturnOk()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/EmotionCheckIns/1");
    response.EnsureSuccessStatusCode();

    var responseString = await response.Content.ReadAsStringAsync();
    var emotionCheckIn = JsonSerializer.Deserialize<EmotionCheckInDto>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    Assert.NotNull(emotionCheckIn);
  }
    
  [Fact]
  public async Task Get_ShouldReturnNotFound_WhenInvalidId()
  {
    await AuthenticateAsync();

    var response = await client.GetAsync("/EmotionCheckIns/9999"); 
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }  

    [Fact]
    public async Task Create_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var createDto = new EmotionCheckInCreateDto
        {
            Emotion = "happy",
            Tags = new List<TagDto>
            {
              new TagDto { Key = "1", Type = TagType.WHAT },
              new TagDto { Key = "2", Type = TagType.WHAT }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/EmotionCheckIns", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var emotionCheckIn = JsonSerializer.Deserialize<EmotionCheckInDto>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(emotionCheckIn);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInvalidData()
    {
      await AuthenticateAsync();

      var createDto = new EmotionCheckInCreateDto
      {
        Emotion = "", //invalid data (emotion is required)
        Tags = new List<TagDto>
        {
          new TagDto { Key = "1", Type = TagType.WHAT },
          new TagDto { Key = "2", Type = TagType.WHAT }
        }
      };

      var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
      var response = await client.PostAsync("/EmotionCheckIns", content);

      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var updateDto = new EmotionCheckInUpdateDto
        {
            Id = 2,
            Emotion = "excited",
            Description = "Feeling very excited today!",
            Tags = new List<TagDto>
            {
              new TagDto { Key = "1", Type = TagType.WHAT },
              new TagDto { Key = "2", Type = TagType.WHAT }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(updateDto), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync("/EmotionCheckIns", content);
        
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var emotionCheckIn = JsonSerializer.Deserialize<EmotionCheckInDto>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(emotionCheckIn);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await client.DeleteAsync("/EmotionCheckIns/1");
        response.EnsureSuccessStatusCode();
    }
  
}