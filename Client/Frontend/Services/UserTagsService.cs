using System.Text.Json;
using API.DTO;
using Frontend.Utils;

namespace Frontend.Services.Interfaces;

public class UserTagsService (AuthedClient httpClient) : IUserTagsService
{
  public async Task<List<TagDTO>> GetAll()
  {
    var response = await httpClient.GetAsync("UserTags");

    return await new ApiParsingUtils<List<TagDTO>>().Process(response) ?? new List<TagDTO>();
  }
}