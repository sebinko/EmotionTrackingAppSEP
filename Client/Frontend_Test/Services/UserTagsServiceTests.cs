using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DTO;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend_Test.Services
{
  [TestFixture]
  public class UserTagsServiceTests
  {
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private Mock<AuthenticationStateProvider> _mockAuthenticationStateProvider;
    private UserTagsService _userTagsService;

    [SetUp]
    public void SetUp()
    {
      _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
      _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
      {
        BaseAddress = new System.Uri("http://localhost")
      };
      _mockAuthenticationStateProvider = new Mock<AuthenticationStateProvider>();

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, "Test User"),
        new Claim("Token", "fake-jwt-token")
      };
      var identity = new ClaimsIdentity(claims, "TestAuthType");
      var user = new ClaimsPrincipal(identity);
      var authState = Task.FromResult(new AuthenticationState(user));

      _mockAuthenticationStateProvider
        .Setup(provider => provider.GetAuthenticationStateAsync())
        .Returns(authState);

      var authedClient = new AuthedClient(_httpClient, _mockAuthenticationStateProvider.Object, "http://localhost");
      _userTagsService = new UserTagsService(authedClient);
    }

    [Test]
    public async Task GetAll_ReturnsListOfTagDto()
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StringContent(JsonSerializer.Serialize(new List<TagDto>()))
      };

      _mockHttpMessageHandler.Protected()
        .Setup<Task<HttpResponseMessage>>(
          "SendAsync",
          ItExpr.IsAny<HttpRequestMessage>(),
          ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(response);

      var result = await _userTagsService.GetAll();

      Assert.IsNotNull(result);
      _mockHttpMessageHandler.Protected().Verify(
        "SendAsync",
        Times.Once(),
        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://localhost/UserTags"),
        ItExpr.IsAny<CancellationToken>()
      );
    }
  }
}