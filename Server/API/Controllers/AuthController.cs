using API.Auth;
using API.DTO;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  private AuthUtilities _authUtilities;

  public AuthController(AuthUtilities authUtilities)
  {
    _authUtilities = authUtilities;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
  {
    // TODO reimplement
    var user = new User()
    {
      Id = 1,
      Username = userLoginDTO.Username
    };

    var token = _authUtilities.GenerateJWTToken(user);

    return Ok(new { token });
  }
}