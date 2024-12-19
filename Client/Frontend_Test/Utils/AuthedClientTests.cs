using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend.Utils.Tests
{
    [TestFixture]
    public class AuthedClientTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private Mock<AuthenticationStateProvider> _mockAuthenticationStateProvider;
        private HttpClient _httpClient;
        private AuthedClient _authedClient;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockAuthenticationStateProvider = new Mock<AuthenticationStateProvider>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _authedClient = new AuthedClient(_httpClient, _mockAuthenticationStateProvider.Object, "http://localhost");
        }

        [Test]
        public async Task GetAsync_UserNotAuthenticated_ThrowsException()
        {
            var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _mockAuthenticationStateProvider
                .Setup(provider => provider.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);
            
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authedClient.GetAsync("/test"));
            Assert.AreEqual("User is not authenticated", ex.Message);
        }

        [Test]
        public async Task GetAsync_UserAuthenticated_ReturnsResponse()
        {
            var claims = new[] { new Claim("Token", "test-token") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var authState = new AuthenticationState(new ClaimsPrincipal(identity));
            _mockAuthenticationStateProvider
                .Setup(provider => provider.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            
            var response = await _authedClient.GetAsync("/test");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Bearer", _httpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.AreEqual("test-token", _httpClient.DefaultRequestHeaders.Authorization.Parameter);
        }
    }
}