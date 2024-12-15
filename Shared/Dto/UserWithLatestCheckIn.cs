namespace DTO;

public class UserWithLatestCheckIn
{
  public required UserReturnDto User { get; set; }
  public required string LatestCheckIn { get; set; }
}