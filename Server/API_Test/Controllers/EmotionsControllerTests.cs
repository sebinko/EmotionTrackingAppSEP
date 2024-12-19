using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Protobuf.Services.Interfaces;
using System.Collections.Generic;

namespace API_Test.Controllers
{
  [TestFixture]
  public class EmotionsControllerTests
  {
    private Mock<IEmotionsService> _emotionsServiceMock;
    private EmotionsController _controller;

    [SetUp]
    public void SetUp()
    {
      _emotionsServiceMock = new Mock<IEmotionsService>();
      _controller = new EmotionsController(_emotionsServiceMock.Object);

      var claimsIdentity = new ClaimsIdentity();
      claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
      _controller.ControllerContext.HttpContext = new DefaultHttpContext();
      _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
    }

    [Test]
    public async Task Get_ShouldReturnOk_WithEmotions()
    {
      var emotions = new List<EmotionDto> { new EmotionDto { Emotion = "Joy", Color = "Yellow", Description = "Feeling happy" } };
      _emotionsServiceMock.Setup(service => service.GetAll(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(emotions);

      var result = await _controller.Get("Joy", "Yellow");

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(emotions, okResult.Value);
    }
  }
}