using System.ComponentModel.DataAnnotations;

namespace DTO;

public class TagDto
{
  [Required] public required string Key { get; set; }
  [Required] public required TagType Type { get; set; }
}

public enum TagType
{
  WHAT,
  WITH,
  WHERE
}