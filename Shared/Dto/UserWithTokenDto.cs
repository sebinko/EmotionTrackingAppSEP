namespace DTO;

public class UserWithTokenDto
{
  public required UserReturnDto User { get; set; }
  public required string Token { get; set; }
}