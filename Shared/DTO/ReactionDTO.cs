namespace API.DTO;

public class ReactionDTO
{
  public string Emoji { get; set; }
  public int UserId { get; set; }
  public int EmotionCheckInId { get; set; }
  public string CreatedAt { get; set; }
  public string UpdatedAt { get; set; }
}