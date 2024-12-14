using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class UserFriendshipController (IUserFriendsService userFriendsService) : ControllerBase
{
  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreateFriendship(CreateFriendshipDto createFriendshipDTO)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    await userFriendsService.CreateFriendship(int.Parse(userId), createFriendshipDTO.user2Id);
      
    return Ok();
  }
  
  [HttpDelete]
  [Authorize]
  public async Task<IActionResult> RemoveFriendship(RemoveFriendshipDto removeFriendshipDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    await userFriendsService.RemoveFriendship(int.Parse(userId), removeFriendshipDto.user2Id);
      
    return Ok();
  }
}