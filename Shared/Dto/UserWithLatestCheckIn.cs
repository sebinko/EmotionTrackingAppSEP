namespace DTO;

public class UserWithLatestCheckIn
{
  public required UserReturnDto User { get; set; }
  public required EmotionCheckInSimpleDto LatestCheckIn { get; set; }
}