using System.Text.Json;
using DTO;
using Frontend.Utils;

namespace Frontend.Services.Interfaces;

public class UserTagsService (AuthedClient httpClient) : IUserTagsService
{
  public async Task<List<TagDto>> GetAll()
  {
    var response = await httpClient.GetAsync("UserTags");

    return await new ApiParsingUtils<List<TagDto>>().Process(response) ?? new List<TagDto>();
  }
}