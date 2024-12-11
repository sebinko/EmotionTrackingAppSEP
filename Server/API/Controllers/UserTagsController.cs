using System.Security.Claims;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserTagsController (UserTagsService userTagsService) : ControllerBase

{
  [HttpGet]
  [Authorize]
  public IActionResult GetAllTags()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    var userReturnDto = new UserReturnDTO
    {
      Id = int.Parse(userId),
      Username = "",
      Email = ""
    };

    var tags = userTagsService.GetAllTags(userReturnDto);

    return Ok(tags);
  }
  
}