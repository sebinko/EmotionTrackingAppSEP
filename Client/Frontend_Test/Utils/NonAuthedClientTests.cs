using System.Net;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Frontend.Utils.Tests
{
    [TestFixture]
    public class NonAuthedClientTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private NonAuthedClient _nonAuthedClient;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _nonAuthedClient = new NonAuthedClient(_httpClient, "http://localhost");
        }

        [Test]
        public async Task GetAsync_ReturnsResponse()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            
            var response = await _nonAuthedClient.GetAsync("/test");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task PatchAsync_WithHeaders_ReturnsResponse()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var headers = new Dictionary<string, string> { { "Custom-Header", "HeaderValue" } };
            var content = new StringContent("Test content");
            
            var response = await _nonAuthedClient.PatchAsync("/test", content, headers);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("HeaderValue", _httpClient.DefaultRequestHeaders.GetValues("Custom-Header").First());
        }
    }
}