using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserTagsController(IUserTagsService userTagsService) : ControllerBase

{
  [HttpGet]
  [Authorize]
  public IActionResult GetAllTags()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    var tags = userTagsService.GetAllTags(int.Parse(userId));

    return Ok(tags);
  }
}