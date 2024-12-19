using System.Net;
using System.Text.Json;
using DTO;
using Frontend.Services;
using Frontend.Utils;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend_Test.Services
{
    [TestFixture]
    public class EmotionsServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private NonAuthedClient _httpClient;
        private EmotionsService _emotionsService;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new NonAuthedClient(new HttpClient(_mockHttpMessageHandler.Object), "http://localhost");
            _emotionsService = new EmotionsService(_httpClient);
        }

        [Test]
        public async Task GetAll_ReturnsListOfEmotionDto()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<EmotionDto>
                {
                    new EmotionDto { Emotion = "Happy", Color = "Yellow", Description = "Feeling happy" },
                    new EmotionDto { Emotion = "Sad", Color = "Blue", Description = "Feeling sad" }
                }), System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var result = await _emotionsService.GetAll(null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Sad", result[1].Emotion);
        }

        [Test]
        public async Task GetAll_WithQueryParameters_ReturnsFilteredEmotionDto()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new List<EmotionDto>
                {
                    new EmotionDto { Emotion = "Happy", Color = "Yellow", Description = "Feeling happy" }
                }), System.Text.Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var result = await _emotionsService.GetAll("Happy", "Yellow");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Yellow", result[0].Color);
        }
    }
}