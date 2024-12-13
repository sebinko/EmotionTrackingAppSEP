using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class EmotionCheckInDTO
{
  public string Emotion { get; set; }
  public int? Id { get; set; }
  public string? Description { get; set; }
  public int? UserId { get; set; }
  public string CreatedAt { get; set; }
  public string UpdatedAt { get; set; }
  
  public List<TagDTO> Tags { get; set; } = new();
}