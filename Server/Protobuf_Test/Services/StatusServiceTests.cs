using System;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.Services;
using Protobuf.Status;

namespace Protobuf_Test.Services
{
  [TestFixture]
  public class StatusServiceTests
  {
    private Mock<Protobuf.Status.StatusService.StatusServiceClient> _mockClient;
    private Protobuf.Services.StatusService _statusService;

    [SetUp]
    public void SetUp()
    {
      _mockClient = new Mock<Protobuf.Status.StatusService.StatusServiceClient>();
      _statusService = new Protobuf.Services.StatusService();
    }

    [Test]
    public void GetStatusMethod_StatusNotOk_ThrowsException()
    {
      var statusReply = new StatusMessage { Status = "Error", Message = "Error occurred", Data = "Some data" };
      _mockClient.Setup(c => c.GetStatusMethod(It.IsAny<StatusRequest>(), It.IsAny<CallOptions>()))
        .Returns(statusReply);

      var ex = Assert.Throws<Exception>(() => _statusService.GetStatusMethod());
      Assert.AreEqual("Error occurred : Some data", ex.Message);
    }

    [Test]
    public void GetStatusMethod_StatusOk_DoesNotThrowException()
    {
      var statusReply = new StatusMessage { Status = "OK" };
      _mockClient.Setup(c => c.GetStatusMethod(It.IsAny<StatusRequest>(), It.IsAny<CallOptions>()))
        .Returns(statusReply);

      Assert.DoesNotThrow(() => _statusService.GetStatusMethod());
    }
  }
}