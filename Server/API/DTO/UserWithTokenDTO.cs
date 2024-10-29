using Entities;

namespace API.DTO;

public class UserWithTokenDTO
{
  public required UserReturnDTO User { get; set; }
  public required string Token { get; set; }
}