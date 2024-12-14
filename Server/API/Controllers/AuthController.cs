using System.Security.Claims;
using API.Auth;
using API.Exceptions;
using DTO;
using API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
  AuthUtilities authUtilities,
  IUsersService usersService,
  PasswordHasherUtil passwordHasherUtil) : ControllerBase
{
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDTO)
  {
    var user =
      await usersService.GetByUsernameAndPassword(userLoginDTO.Username,
        passwordHasherUtil.HashPassword(userLoginDTO.Password));

    if (user == null)
    {
      throw new UnauthorizedException("Invalid username or password");
    }

    var token = authUtilities.GenerateJwtToken(user);

    return Ok(new UserWithTokenDto
    {
      User = new UserReturnDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Streak = user.Streak,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
      },
      Token = token
    });
  }

  [HttpPost("register")]
  public async Task<IActionResult> Create([FromBody] UserRegisterDto userRegisterDto)
  {
    var user = await usersService.Create(userRegisterDto);

    return Ok(new UserWithTokenDto
    {
      User = new UserReturnDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Streak = user.Streak,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
      },
      Token = authUtilities.GenerateJwtToken(user)
    });
  }

  [HttpPatch("change-password")]
  [Authorize]
  public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDTO)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    changePasswordDTO.NewPassword = passwordHasherUtil.HashPassword(changePasswordDTO.NewPassword);

    var user = await usersService.ChangePassword(int.Parse(userId), changePasswordDTO);

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

  [HttpGet()]
  [Authorize]
  public async Task<IActionResult> Get()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }

    var user = await usersService.GetById(int.Parse(userId));

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