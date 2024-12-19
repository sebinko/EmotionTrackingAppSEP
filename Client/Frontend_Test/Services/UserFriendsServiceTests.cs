using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DTO;
using Frontend.Services;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend_Test.Services
{
    [TestFixture]
    public class UserFriendsServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Mock<AuthenticationStateProvider> _mockAuthenticationStateProvider;
        private UserFriendsService _userFriendsService;

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
            _userFriendsService = new UserFriendsService(authedClient);
        }

        [Test]
        public async Task CreateFriendship_CallsPostAsync()
        {
            var user2Username = "testUser";
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            await _userFriendsService.CreateFriendship(user2Username);

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == "http://localhost/UserFriendship"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task RemoveFriendship_CallsDeleteAsync()
        {
            var user2Username = "testUser";
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            await _userFriendsService.RemoveFriendship(user2Username);

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString() == $"http://localhost/UserFriendship/{user2Username}"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task GetFriends_ReturnsListOfUserWithLatestCheckIn()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<UserWithLatestCheckIn>()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _userFriendsService.GetFriends();

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://localhost/UserFriendship"),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}