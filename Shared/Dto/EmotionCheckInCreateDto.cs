using System.ComponentModel.DataAnnotations;

namespace DTO;

public class EmotionCheckInCreateDto
{
  [Required] public required string Emotion { get; set; }
  
  public string? Description { get; set; }
  public List<TagDto> Tags { get; set; }
}