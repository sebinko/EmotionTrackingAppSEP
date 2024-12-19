using Protobuf.Services;

namespace API_Integration_Tests;

public class StatusServiceIntegrationTests
{
  private readonly StatusService service;

  public StatusServiceIntegrationTests()
  {
    service = new StatusService();
  }

  [Fact]
  public void GetStatusMethod_ShouldReturnOKStatus()
  {
    var exception = Record.Exception(() => service.GetStatusMethod());
    
    Assert.Null(exception);
  }
}