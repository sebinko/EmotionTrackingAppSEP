using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SharedUtil;
using Frontend.Utils;

namespace Frontend.Tests.Utils
{
    [TestFixture]
    public class ApiParsingUtilsTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        }

        [Test]
        public async Task Process_WithEmptyContent_ReturnsNull()
        {
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            };
            var apiParsingUtils = new ApiParsingUtils<object>();
            
            var result = await apiParsingUtils.Process(responseMessage);
            
            Assert.IsNull(result);
        }

        [Test]
        public async Task Process_WithValidContent_ReturnsDeserializedObject()
        {
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("{\"Property\":\"Value\"}")
            };
            var apiParsingUtils = new ApiParsingUtils<TestDto>();
            
            var result = await apiParsingUtils.Process(responseMessage);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Value", result.Property);
        }

        public class TestDto
        {
            public string Property { get; set; }
        }
    }
}