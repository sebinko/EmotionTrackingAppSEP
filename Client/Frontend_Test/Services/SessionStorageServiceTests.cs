using System.Threading.Tasks;
using Blazored.SessionStorage;
using Frontend.Services;
using Moq;
using NUnit.Framework;

namespace Frontend_Test.Services
{
  [TestFixture]
  public class SessionStorageServiceTests
  {
    private Mock<ISessionStorageService> _mockSessionStorage;
    private SessionStorageService _sessionStorageService;

    [SetUp]
    public void SetUp()
    {
      _mockSessionStorage = new Mock<ISessionStorageService>();
      _sessionStorageService = new SessionStorageService(_mockSessionStorage.Object);
    }

    [Test]
    public async Task GetItem_ReturnsItem()
    {
      var key = "testKey";
      var value = "testValue";

      _mockSessionStorage.Setup(s => s.GetItemAsync<string>(key, default))
        .ReturnsAsync(value);

      var result = await _sessionStorageService.GetItem<string>(key);

      Assert.AreEqual(value, result);
    }

    [Test]
    public async Task SetItem_StoresItem()
    {
      var key = "testKey";
      var value = "testValue";

      _mockSessionStorage.Setup(s => s.SetItemAsync(key, value, default))
        .Returns(new ValueTask());

    }

    [Test]
    public async Task RemoveItem_RemovesItem()
    {
      var key = "testKey";

      _mockSessionStorage.Setup(s => s.RemoveItemAsync(key, default))
        .Returns(new ValueTask());

      await _sessionStorageService.RemoveItem(key);

      _mockSessionStorage.Verify(s => s.RemoveItemAsync(key, default), Times.Once);
    }
  }
}