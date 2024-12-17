using DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;

namespace Frontend.Services;

public class UsersService (AuthedClient httpClient) : IUsersService
{
  public async Task<UserReturnDto?> GetById(int id)
  {
    var response = await httpClient.GetAsync($"users/{id}");

    return await new ApiParsingUtils<UserReturnDto>().Process(response);
  }
}