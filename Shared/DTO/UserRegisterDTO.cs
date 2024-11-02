using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class UserRegisterDTO
{
  [Required] public required string Username { get; set; }
  [Required] public required string Password { get; set; }
  [Required] public required string Email { get; set; }
}