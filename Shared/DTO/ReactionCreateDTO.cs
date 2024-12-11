namespace API.DTO;

public class ReactionCreateDTO
{
  public string? Emoji { get; set; }
  public int UserId { get; set; }
  public int EmotionCheckInId { get; set; }
  
}