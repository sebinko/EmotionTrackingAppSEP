using API.DTO;

namespace Frontend.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Frontend.Services.Interfaces;

public class AppAuthenticationStateProvider : AuthenticationStateProvider
{
  private readonly IAuthService _authService;

  public AppAuthenticationStateProvider(IAuthService authService)
  {
    _authService = authService;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var userWithToken = await _authService.GetUser();
    ClaimsIdentity identity;

    if (userWithToken?.User != null)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, userWithToken.User.Username),
        new Claim(ClaimTypes.Email, userWithToken.User.Email)
      };

      identity = new ClaimsIdentity(claims, "apiauth_type");
    }
    else
    {
      identity = new ClaimsIdentity();
    }

    var user = new ClaimsPrincipal(identity);
    return new AuthenticationState(user);
  }

  public void NotifyUserAuthentication(UserWithTokenDTO userWithToken)
  {
    var identity = new ClaimsIdentity(new[]
    {
      new Claim(ClaimTypes.Name, userWithToken.User.Username),
      new Claim(ClaimTypes.Email, userWithToken.User.Email)
    }, "apiauth_type");

    var user = new ClaimsPrincipal(identity);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  public void NotifyUserLogout()
  {
    var identity = new ClaimsIdentity();
    var user = new ClaimsPrincipal(identity);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }
} 