using System.ComponentModel.DataAnnotations;

namespace DTO;

public class RemoveFriendshipDto
{
  [Required] public required string user2UserName { get; set; }
}