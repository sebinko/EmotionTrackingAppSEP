using System.Security.Claims;
using API.Exceptions;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class UserFriendshipController (IUserFriendsService userFriendsService, IUsersService usersService) : ControllerBase
{
  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetAllFriends()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    var friends = await userFriendsService.GetFriends(int.Parse(userId));
      
    return Ok(friends);
  }
  
  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreateFriendship(CreateFriendshipDto createFriendshipDTO)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    var user2 = await usersService.GetByUsername(createFriendshipDTO.user2UserName);
    
    if (user2 == null)
    {
      throw new NotFoundException("User not found");
    }

    await userFriendsService.CreateFriendship(int.Parse(userId), user2.Id);
      
    return Ok();
  }
  
  [HttpDelete("{user2Username}")]
  [Authorize]
  public async Task<IActionResult> RemoveFriendship(string user2Username)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    var user2 = await usersService.GetByUsername(user2Username);
    
    if (user2 == null)
    {
      throw new NotFoundException("User not found");
    }
    
    await userFriendsService.RemoveFriendship(int.Parse(userId), user2.Id);
      
    return Ok();
  }
}