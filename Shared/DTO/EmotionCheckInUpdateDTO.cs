using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class EmotionCheckInUpdateDTO
{
  [Required] public required int id { get; set; }
  [Required] public required string Emotion { get; set; }
  public string Description { get; set; }
  
  public List<TagDTO> Tags { get; set; }
}