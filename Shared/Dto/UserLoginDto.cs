using System.ComponentModel.DataAnnotations;

namespace DTO;

public class UserLoginDto
{
  [Required] public required string Username { get; set; }

  [Required] public required string Password { get; set; }
}