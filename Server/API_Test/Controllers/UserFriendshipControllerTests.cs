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
    public class UserFriendshipControllerTests
    {
        private Mock<IUserFriendsService> _userFriendsServiceMock;
        private Mock<IUsersService> _usersServiceMock;
        private UserFriendshipController _controller;

        [SetUp]
        public void SetUp()
        {
            _userFriendsServiceMock = new Mock<IUserFriendsService>();
            _usersServiceMock = new Mock<IUsersService>();
            _controller = new UserFriendshipController(_userFriendsServiceMock.Object, _usersServiceMock.Object);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        [Test]
        public async Task GetAllFriends_ShouldReturnOk_WithFriends()
        {
          var friends = new List<UserWithLatestCheckIn> 
          { 
            new UserWithLatestCheckIn 
            { 
              User = new UserReturnDto { Id = 1, Username = "user1", Email = "user1@example.com" }, 
              LatestCheckIn = DateTime.Now.ToString("o") 
            } 
          };
          _userFriendsServiceMock.Setup(service => service.GetFriends(It.IsAny<int>())).ReturnsAsync(friends);

            var result = await _controller.GetAllFriends();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(friends, okResult.Value);
        }

        [Test]
        public async Task CreateFriendship_ShouldReturnOk_WhenFriendshipCreated()
        {
            var createFriendshipDto = new CreateFriendshipDto { user2UserName = "user2" };
            var user2 = new UserReturnDto { Id = 2, Username = "user2", Email = "user2@example.com" };
            _usersServiceMock.Setup(service => service.GetByUsername(It.IsAny<string>())).ReturnsAsync(user2);
            _userFriendsServiceMock.Setup(service => service.CreateFriendship(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateFriendship(createFriendshipDto);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void CreateFriendship_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            var createFriendshipDto = new CreateFriendshipDto { user2UserName = "user2" };
            _usersServiceMock.Setup(service => service.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserReturnDto)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _controller.CreateFriendship(createFriendshipDto));
        }

        [Test]
        public async Task RemoveFriendship_ShouldReturnOk_WhenFriendshipRemoved()
        {
          var user2 = new UserReturnDto { Id = 2, Username = "user2", Email = "user2@example.com" };
          _usersServiceMock.Setup(service => service.GetByUsername(It.IsAny<string>())).ReturnsAsync(user2);
            _userFriendsServiceMock.Setup(service => service.RemoveFriendship(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.RemoveFriendship("user2");

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void RemoveFriendship_ShouldThrowNotFoundException_WhenUserNotFound()
        {
          _usersServiceMock.Setup(service => service.GetByUsername(It.IsAny<string>())).ReturnsAsync((UserReturnDto)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _controller.RemoveFriendship("user2"));
        }
    }
}