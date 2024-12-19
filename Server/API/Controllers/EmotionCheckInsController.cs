using System.Security.Claims;
using API.Exceptions;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class EmotionCheckInsController(IEmotionCheckInService emotionCheckInService, IReactionService reactionService) : ControllerBase
{
  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetAll()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    return Ok(await emotionCheckInService.GetAll(int.Parse(userId)));
  }
  
  [HttpPost("by-tags")]
  [Authorize]
  public async Task<IActionResult> GetByTags([FromBody] GetEmotionCheckInByTagsDto getEmotionCheckInByTagsDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    return Ok(await emotionCheckInService.GetByTags(getEmotionCheckInByTagsDto.Tags, int.Parse(userId)));
  }

  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    return Ok(await emotionCheckInService.GetById(id, int.Parse(userId)));
  }
  
  [HttpGet("{id}/reactions")]
  [Authorize]
  public async Task<IActionResult> GetReactions(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    var emotionCheckIn = await emotionCheckInService.GetById(id, int.Parse(userId));
    
    if (emotionCheckIn == null)
    {
      throw new NotFoundException("EmotionCheckIn not found");
    }

    return Ok(await reactionService.GetByReactionsByEmotionCheckIn(id));
  }


  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Create([FromBody] EmotionCheckInCreateDto emotionCheckInDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    return Ok(await emotionCheckInService.Create(emotionCheckInDto, int.Parse(userId)));
  }

  [HttpPatch]
  public async Task<IActionResult> Update(
    [FromBody] EmotionCheckInUpdateDto emotionCheckInDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    var emotionCheckIn = await emotionCheckInService.GetById(emotionCheckInDto.Id, int.Parse(userId));
    
    if (emotionCheckIn == null)
    {
      throw new NotFoundException("EmotionCheckIn not found");
    }
    
    if (emotionCheckIn.UserId != int.Parse(userId))
    {
      throw new UnauthorizedException("EmotionCheckIn does not belong to the user");
    }

    return Ok(await emotionCheckInService.Update(emotionCheckInDto, int.Parse(userId)));
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> Delete(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    var emotionCheckIn = await emotionCheckInService.GetById(id, int.Parse(userId));
    
    if (emotionCheckIn == null)
    {
      throw new NotFoundException("EmotionCheckIn not found");
    }
    
    if (emotionCheckIn.UserId != int.Parse(userId))
    {
      throw new UnauthorizedException("EmotionCheckIn does not belong to the user");
    }
    
    return Ok(await emotionCheckInService.Delete(id));
  }
}