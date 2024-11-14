using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class EmotionCheckInCreateDTO
{
  [Required] public required string Emotion { get; set; }
  [Required] public required int? UserId { get; set; }
}