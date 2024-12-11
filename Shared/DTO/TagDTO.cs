using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class TagDTO
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