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
using Frontend.Services.Interfaces;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend_Test.Services
{
    [TestFixture]
    public class EmotionCheckInServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Mock<AuthenticationStateProvider> _mockAuthenticationStateProvider;
        private EmotionCheckInService _emotionCheckInService;

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
            _emotionCheckInService = new EmotionCheckInService(authedClient);
        }

        [Test]
        public async Task GetAll_ReturnsListOfEmotionCheckInDto()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<EmotionCheckInDto>()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.GetAll();

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://localhost/EmotionCheckIns"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task GetByTags_ReturnsListOfEmotionCheckInDto()
        {
            var getEmotionCheckInByTags = new GetEmotionCheckInByTagsDto();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<EmotionCheckInDto>()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.GetByTags(getEmotionCheckInByTags);

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == "http://localhost/EmotionCheckIns/by-tags"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task Get_ReturnsEmotionCheckInDto()
        {
            var id = 1;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new EmotionCheckInDto()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.Get(id);

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == $"http://localhost/EmotionCheckIns/{id}"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task Create_ReturnsEmotionCheckInDto()
        {
            var emotionCheckIn = new EmotionCheckInCreateDto
            {
                Emotion = "Happy"
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new EmotionCheckInDto()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.Create(emotionCheckIn);

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == "http://localhost/EmotionCheckIns"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task Update_ReturnsEmotionCheckInDto()
        {
            var id = 1;
            var emotionCheckIn = new EmotionCheckInUpdateDto
            {
                Id = id,
                Emotion = "Happy"
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new EmotionCheckInDto()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.Update(id, emotionCheckIn);

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Patch && req.RequestUri.ToString() == "http://localhost/EmotionCheckIns"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task Delete_ReturnsEmotionCheckInDto()
        {
            var id = 1;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new EmotionCheckInDto()))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var result = await _emotionCheckInService.Delete(id);

            Assert.IsNotNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString() == $"http://localhost/EmotionCheckIns/{id}"),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}