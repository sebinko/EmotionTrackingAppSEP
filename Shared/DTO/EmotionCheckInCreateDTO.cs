using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class EmotionCheckInCreateDTO
{
  [Required] public required string Emotion { get; set; }
  
  public List<string> Tags { get; set; }
}