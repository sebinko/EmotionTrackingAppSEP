using System.ComponentModel.DataAnnotations;

namespace DTO;

public class UserRegisterDto
{
  [Required] public required string Username { get; set; }
  [Required] public required string Password { get; set; }
  [Required] public required string Email { get; set; }
}