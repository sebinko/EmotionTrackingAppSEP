using Protobuf.Services;

namespace API_Integration_Tests;

public class EmotionsServiceIntegrationTests
{
  private readonly EmotionsService service;

  public EmotionsServiceIntegrationTests()
  {
    service = new EmotionsService();
  }

  [Fact]
  public async Task GetAll_ShouldReturnAllEmotions()
  {
    var result = await service.GetAll(null, null);

    Assert.NotNull(result);
    Assert.NotEmpty(result);
  }
}