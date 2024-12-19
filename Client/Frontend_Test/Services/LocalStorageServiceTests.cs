using System.Text.Json;
using Frontend.Services;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework;

namespace Frontend_Test.Services
{
    [TestFixture]
    public class LocalStorageServiceTests
    {
        private Mock<IJSRuntime> _mockJsRuntime;
        private LocalStorageService _localStorageService;

        [SetUp]
        public void SetUp()
        {
            _mockJsRuntime = new Mock<IJSRuntime>();
            _localStorageService = new LocalStorageService(_mockJsRuntime.Object);
        }

        [Test]
        public async Task GetItem_ReturnsString()
        {
            var key = "testKey";
            var value = "testValue";

            _mockJsRuntime.Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(value);

            var result = await _localStorageService.GetItem<string>(key);

            Assert.AreEqual(value, result);
        }

        [Test]
        public async Task GetItem_ReturnsDeserializedObject()
        {
            var key = "testKey";
            var value = new TestObject { Name = "testName" };
            var json = JsonSerializer.Serialize(value);

            _mockJsRuntime.Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(json);

            var result = await _localStorageService.GetItem<TestObject>(key);

            Assert.AreEqual(value.Name, result.Name);
        }
        
        public class TestObject
        {
            public string Name { get; set; }
        }
    }
}