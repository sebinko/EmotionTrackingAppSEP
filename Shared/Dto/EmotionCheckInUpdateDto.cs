using System.ComponentModel.DataAnnotations;

namespace DTO;

public class EmotionCheckInUpdateDto
{
  [Required] public required int Id { get; set; }
  [Required] public required string Emotion { get; set; }
  public string Description { get; set; }
  
  public List<TagDto> Tags { get; set; }
}