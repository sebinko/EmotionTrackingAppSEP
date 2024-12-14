using API.DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;

namespace Frontend.Services;

public class FriendshipService(AuthedClient httpClient) : IFriendshipService
{
  public async Task<List<FriendshipDTO>?> GetAll()
  {
    var response = await httpClient.GetAsync("Friendships");
    
    return await new ApiParsingUtils<List<FriendshipDTO>>().Process(response);
  }
}