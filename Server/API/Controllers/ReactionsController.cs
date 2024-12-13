using System.Security.Claims;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Entities;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReactionsController(ReactionService reactionService) : ControllerBase
{
  [HttpPost]
  [Authorize]
  public async Task<ActionResult> Create([FromBody] ReactionCreateDTO reactionDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
    if (userId == null)
    {
      return Unauthorized();
    }

    return Ok(await reactionService.Create(reactionDto, int.Parse(userId)));
  }
  [HttpDelete]
  [Authorize]
  public async Task<ActionResult> Delete([FromBody] ReactionDeleteDTO reactionDto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
    if (userId == null)
    {
      return Unauthorized();
    }

    await reactionService.Delete(reactionDto, int.Parse(userId));
    return Ok();
  }
}