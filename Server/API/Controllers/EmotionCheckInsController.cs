﻿using System.Security.Claims;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionCheckInsController(EmotionCheckInService emotionCheckInService) : ControllerBase
{
  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Create([FromBody] EmotionCheckInCreateDTO emotionCheckInDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
    if (userId == null)
    {
      return Unauthorized();
    }

    return Ok(await emotionCheckInService.Create(emotionCheckInDto, int.Parse(userId)));
  }

  [HttpDelete]
  [Authorize]
  public async Task<IActionResult> Delete([FromBody] int id)
  {
    // TODO CHECK IF IT BELONGS TO THE USER
    return Ok(await emotionCheckInService.Delete(id));
  }
}