using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers;
using API.Exceptions;
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
  public class EmotionCheckInsControllerTests
  {
    private Mock<IEmotionCheckInService> _emotionCheckInServiceMock;
    private Mock<IReactionService> _reactionServiceMock;
    private EmotionCheckInsController _controller;

    [SetUp]
    public void SetUp()
    {
      _emotionCheckInServiceMock = new Mock<IEmotionCheckInService>();
      _reactionServiceMock = new Mock<IReactionService>();
      _controller = new EmotionCheckInsController(_emotionCheckInServiceMock.Object, _reactionServiceMock.Object);
      
      

      var claimsIdentity = new ClaimsIdentity();
      claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
      _controller.ControllerContext.HttpContext = new DefaultHttpContext();
      _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
    }

    [Test]
    public async Task GetAll_ShouldReturnOk_WithEmotionCheckIns()
    {
      var emotionCheckIns = new List<EmotionCheckInDto> { new EmotionCheckInDto { Id = 1, UserId = 1, Emotion = "Happy" } };
      _emotionCheckInServiceMock.Setup(service => service.GetAll(It.IsAny<int>())).ReturnsAsync(emotionCheckIns);

      var result = await _controller.GetAll();

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(emotionCheckIns, okResult.Value);
    }

    [Test]
    public async Task GetByTags_ShouldReturnOk_WithEmotionCheckIns()
    {
      var emotionCheckIns = new List<EmotionCheckInDto> { new EmotionCheckInDto { Id = 1, UserId = 1, Emotion = "Happy" } };
      var getEmotionCheckInByTagsDto = new GetEmotionCheckInByTagsDto
      {
        Tags = new List<TagDto>
        {
          new TagDto
          {
            Key = "Key1",
            Type = TagType.WHAT
          }
        }
      };
      _emotionCheckInServiceMock.Setup(service => service.GetByTags(It.IsAny<List<TagDto>>(), It.IsAny<int>())).ReturnsAsync(emotionCheckIns);

      var result = await _controller.GetByTags(getEmotionCheckInByTagsDto);

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(emotionCheckIns, okResult.Value);
    }

    [Test]
    public async Task Get_ShouldReturnOk_WithEmotionCheckIn()
    {
      var emotionCheckIn = new EmotionCheckInDto { Id = 1, UserId = 1, Emotion = "Happy" };
      _emotionCheckInServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(emotionCheckIn);

      var result = await _controller.Get(1);

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(emotionCheckIn, okResult.Value);
    }

    [Test]
    public async Task Create_ShouldReturnOk_WithNewEmotionCheckIn()
    {
      var emotionCheckInCreateDto = new EmotionCheckInCreateDto { Emotion = "Happy" };
      var newEmotionCheckIn = new EmotionCheckInDto { Id = 1, UserId = 1, Emotion = "Happy" };
      _emotionCheckInServiceMock.Setup(service => service.Create(It.IsAny<EmotionCheckInCreateDto>(), It.IsAny<int>())).ReturnsAsync(newEmotionCheckIn);

      var result = await _controller.Create(emotionCheckInCreateDto);

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(newEmotionCheckIn, okResult.Value);
    }

    [Test]
    public async Task Update_ShouldReturnOk_WithUpdatedEmotionCheckIn()
    {
      var emotionCheckInUpdateDto = new EmotionCheckInUpdateDto { Id = 1, Emotion = "Sad" };
      var updatedEmotionCheckIn = new EmotionCheckInDto { Id = 1, UserId = 1, Emotion = "Sad" };
      _emotionCheckInServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updatedEmotionCheckIn);
      _emotionCheckInServiceMock.Setup(service => service.Update(It.IsAny<EmotionCheckInUpdateDto>(), It.IsAny<int>())).ReturnsAsync(updatedEmotionCheckIn);

      var result = await _controller.Update(emotionCheckInUpdateDto);

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(updatedEmotionCheckIn, okResult.Value);
    }
  }
}