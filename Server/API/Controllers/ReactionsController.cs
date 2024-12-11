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

    reactionDto.UserId = int.Parse(userId);

    return Ok(await reactionService.Create(reactionDto));
  }
}