using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class ChangePasswordDTO
{
  [Required] public required string NewPassword { get; set; }
}