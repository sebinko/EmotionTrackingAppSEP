using System.ComponentModel.DataAnnotations;

namespace DTO;

public class GetEmotionCheckInByTagsDto
{
  [Required] public List<TagDto> Tags { get; set; }
}