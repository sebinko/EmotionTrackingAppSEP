using API.Exceptions;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
  [HttpGet("{id}")]
  [Authorize]
  public async Task<IActionResult> Get([FromRoute] int id)
  {
    var user = await usersService.GetById(id);

    if (user == null)
    {
      throw new NotFoundException("User not found");
    }

    return Ok(new UserReturnDto
    {
      Id = user.Id,
      Username = user.Username,
      Email = user.Email,
      Streak = user.Streak,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt
    });
  }
}