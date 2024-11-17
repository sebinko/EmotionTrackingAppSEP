using Frontend.Services.Interfaces;

namespace Frontend.Utils;

public class AuthTokenHandler : DelegatingHandler
{
  private readonly IAuthService _authService;

  public AuthTokenHandler(IAuthService authService)
  {
    _authService = authService;
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var userWithToken = await _authService.GetUser();
    if (userWithToken?.Token is not null)
    {
      request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userWithToken.Token);
    }

    return await base.SendAsync(request, cancellationToken);
  }
}