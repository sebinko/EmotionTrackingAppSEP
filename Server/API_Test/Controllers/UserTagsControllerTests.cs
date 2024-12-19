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
  public class UserTagsControllerTests
  {
    private Mock<IUserTagsService> _userTagsServiceMock;
    private UserTagsController _controller;

    [SetUp]
    public void SetUp()
    {
      _userTagsServiceMock = new Mock<IUserTagsService>();
      _controller = new UserTagsController(_userTagsServiceMock.Object);

      var claimsIdentity = new ClaimsIdentity();
      claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
      _controller.ControllerContext.HttpContext = new DefaultHttpContext();
      _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
    }

    [Test]
    public void GetAllTags_ShouldReturnOk_WithTags()
    {
      var tags = new List<TagDto>
      {
        new TagDto
        {
          Key = "Key1",
          Type = TagType.WHAT
        }
      };
      _userTagsServiceMock.Setup(service => service.GetAllTags(It.IsAny<int>())).Returns(tags);

      var result = _controller.GetAllTags();

      var okResult = result as OkObjectResult;
      Assert.IsNotNull(okResult);
      Assert.AreEqual(200, okResult.StatusCode);
      Assert.AreEqual(tags, okResult.Value);
    }
  }
}