using System.Net;
using System.Text;
using System.Text.Json;
using DTO;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend_Test.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IStorageService> _mockStorageService;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private NonAuthedClient _httpClient;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _mockStorageService = new Mock<IStorageService>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new NonAuthedClient(new HttpClient(_mockHttpMessageHandler.Object), "http://localhost");
            _authService = new AuthService(_mockStorageService.Object, _httpClient);
        }

        [Test]
        public async Task Register_UserIsRegistered_ReturnsUserWithTokenDto()
        {
            var userRegisterDto = new UserRegisterDto { Username = "testuser", Password = "password", Email = "test@example.com" };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new UserWithTokenDto
                {
                    User = new UserReturnDto { Username = "testuser", Email = "test@example.com", Id = 1, Streak = 5 },
                    Token = "test-token"
                }), Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            
            var result = await _authService.Register(userRegisterDto);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.User.Username);
            Assert.AreEqual("test@example.com", result.User.Email);
            Assert.AreEqual("test-token", result.Token);
        }

        [Test]
        public async Task Login_UserIsAuthenticated_ReturnsUserWithTokenDto()
        {
            var username = "testuser";
            var password = "password";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new UserWithTokenDto
                {
                    User = new UserReturnDto { Username = "testuser", Email = "test@example.com", Id = 1, Streak = 5 },
                    Token = "test-token"
                }), Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            
            var result = await _authService.Login(username, password);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.User.Username);
            Assert.AreEqual("test@example.com", result.User.Email);
            Assert.AreEqual("test-token", result.Token);
        }

        [Test]
        public async Task ChangePassword_PasswordIsChanged_ReturnsUserReturnDto()
        {
            var userWithTokenDto = new UserWithTokenDto
            {
                User = new UserReturnDto { Username = "testuser", Email = "test@example.com", Id = 1, Streak = 5 },
                Token = "test-token"
            };
            var newPassword = "newpassword";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new UserReturnDto
                {
                    Username = "testuser",
                    Email = "test@example.com",
                    Id = 1,
                    Streak = 5
                }), Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            
            var result = await _authService.ChangePassword(userWithTokenDto, newPassword);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.Username);
            Assert.AreEqual("test@example.com", result.Email);
        }
    }
}