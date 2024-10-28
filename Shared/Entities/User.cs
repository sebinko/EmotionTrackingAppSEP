using System.ComponentModel.DataAnnotations;

namespace Entities;

public class User
{
  public int? Id { get; set; }
  [Required] public required string Username { get; set; }
  [Required] public required string Password { get; set; }
  [Required] public required string Email { get; set; }
  public int? Streak { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}