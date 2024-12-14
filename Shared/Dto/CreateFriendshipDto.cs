using System.ComponentModel.DataAnnotations;

namespace DTO;

public class CreateFriendshipDto
{
  [Required] public required int user2Id { get; set; }
}