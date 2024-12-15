using System.ComponentModel.DataAnnotations;

namespace DTO;

public class CreateFriendshipDto
{
  [Required] public required string user2UserName { get; set; }
}