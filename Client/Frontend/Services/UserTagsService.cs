using System.Text.Json;
using API.DTO;
using Frontend.Utils;

namespace Frontend.Services.Interfaces;

public class UserTagsService (AuthedClient httpClient) : IUserTagsService
{
  public async Task<List<TagDTO>> GetAll()
  {
    var response = await httpClient.GetAsync("UserTags");

    var responseData = await response.Content.ReadAsStringAsync();
    
    if(!response.IsSuccessStatusCode)
    {
      throw new Exception(responseData);
    }
    
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    
    return JsonSerializer.Deserialize<List<TagDTO>>(responseData, options);
  }
}