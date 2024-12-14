using System.ComponentModel.DataAnnotations;

namespace DTO;

public class ChangePasswordDto
{
  [Required(AllowEmptyStrings = false)] public required string NewPassword { get; set; }
}