using Entities;

namespace API.DTO;

public class UserWithTokenDTO
{
  public required User User { get; set; }
  public required string Token { get; set; }
}