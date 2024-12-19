using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Protobuf.Services.Interfaces;

namespace API_Test.Controllers
{
    [TestFixture]
    public class ReactionsControllerTests
    {
        private Mock<IReactionService> _reactionServiceMock;
        private ReactionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _reactionServiceMock = new Mock<IReactionService>();
            _controller = new ReactionsController(_reactionServiceMock.Object);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        [Test]
        public async Task Create_ShouldReturnOk_WithNewReaction()
        {
            var reactionCreateDto = new ReactionCreateDto{};
            var newReaction = new ReactionDto{};
            _reactionServiceMock.Setup(service => service.Create(It.IsAny<ReactionCreateDto>(), It.IsAny<int>())).ReturnsAsync(newReaction);

            var result = await _controller.Create(reactionCreateDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(newReaction, okResult.Value);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenReactionDeleted()
        {
            var reactionDeleteDto = new ReactionDeleteDto{};
            _reactionServiceMock.Setup(service => service.Delete(It.IsAny<ReactionDeleteDto>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.Delete(reactionDeleteDto);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIdIsNull()
        {
            var reactionDeleteDto = new ReactionDeleteDto{};
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var result = await _controller.Delete(reactionDeleteDto);

            var unauthorizedResult = result as UnauthorizedResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }
    }
}