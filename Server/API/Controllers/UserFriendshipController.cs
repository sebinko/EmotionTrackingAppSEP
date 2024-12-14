using System.Collections;
using System.Security.Claims;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Users;
using UserFriendsService = Protobuf.Services.UserFriendsService;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class UserFriendshipController (UserFriendsService userFriendsService) : ControllerBase
{
  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreateFriendship(CreateFriendshipDTO createFriendshipDTO)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    await userFriendsService.CreateFriendship(int.Parse(userId), createFriendshipDTO.user2Id);
      
    return Ok();
  }
  
  [HttpGet]
  [Authorize]
  public async Task<IActionResult<List<FriendshipDTO>> GetAllFriendships()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    await userFriendsService.GetAllFriendships(int.Parse(userId));
      
    return Ok();
  }
  
  [HttpDelete]
  [Authorize]
  public async Task<IActionResult> RemoveFriendship(RemoveFriendshipDTO removeFriendshipDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    await userFriendsService.RemoveFriendship(int.Parse(userId), removeFriendshipDto.user2Id);
      
    return Ok();
  }
}