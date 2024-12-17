using System.ComponentModel.DataAnnotations;

namespace DTO;

public class EmotionCheckInSimpleDto
{
  public string Emotion { get; set; }
  public int? Id { get; set; }
  public string UpdatedAt { get; set; }
}