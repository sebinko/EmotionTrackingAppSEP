using System.Security.Claims;
using API.Auth;
using API.DTO;
using API.Utilities;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(AuthUtilities authUtilities, UsersService usersService, PasswordHasherUtil passwordHasherUtil) : ControllerBase
{
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
  {
    var user =
      await usersService.GetByUsernameAndPassword(userLoginDTO.Username, passwordHasherUtil.HashPassword(userLoginDTO.Password));

    if (user == null) return Unauthorized();

    var token = authUtilities.GenerateJWTToken(user);

    return Ok(new UserWithTokenDTO
    {
      User = new UserReturnDTO
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
  public async Task<IActionResult> Create([FromBody] UserRegisterDTO userRegisterDto)
  {
    var user = new User
    {
      Username = userRegisterDto.Username,
      Password = passwordHasherUtil.HashPassword(userRegisterDto.Password),
      Email = userRegisterDto.Email
    };

    user = await usersService.Create(user);

    return Ok(new UserWithTokenDTO
    {
      User = new UserReturnDTO
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Streak = user.Streak,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
      },
      Token = authUtilities.GenerateJWTToken(user)
    });
  }

  [HttpPatch("change-password")]
  [Authorize]
  public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (userId == null)
    {
      return Unauthorized();
    }
    
    changePasswordDTO.NewPassword = passwordHasherUtil.HashPassword(changePasswordDTO.NewPassword);

    var user = await usersService.ChangePassword(int.Parse(userId), changePasswordDTO);

    return Ok(new UserReturnDTO
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