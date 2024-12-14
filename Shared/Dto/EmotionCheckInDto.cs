using System.ComponentModel.DataAnnotations;

namespace DTO;

public class EmotionCheckInDto
{
  public string Emotion { get; set; }
  public int? Id { get; set; }
  public string? Description { get; set; }
  public int? UserId { get; set; }
  public string CreatedAt { get; set; }
  public string UpdatedAt { get; set; }
  
  public List<TagDto> Tags { get; set; } = new();
}