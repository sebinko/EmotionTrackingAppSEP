using System.Text;
using System.Text.Json;
using DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;

namespace Frontend.Services;

public class UserFriendsService (AuthedClient httpClient): IUserFriendsService
{
  public async Task CreateFriendship(string user2Username)
  {
    var json = JsonSerializer.Serialize(new CreateFriendshipDto { user2UserName = user2Username });
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    
    var response = await httpClient.PostAsync("UserFriendship", content);

    await new ApiParsingUtils().Process(response);
  }

  public async Task RemoveFriendship(string user2Username)
  {
    throw new NotImplementedException();
  }

  public async Task<List<UserWithLatestCheckIn>> GetFriends()
  {
    var response = await httpClient.GetAsync($"UserFriendship");
    return await new ApiParsingUtils<List<UserWithLatestCheckIn>>().Process(response);
  }
}