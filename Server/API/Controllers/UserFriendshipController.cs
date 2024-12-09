﻿using System.Security.Claims;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

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
}