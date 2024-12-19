using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using API.Auth;
using API.Exceptions;
using Protobuf.Services.Interfaces;
using DTO;
using API.Utilities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NUnit.Framework;
using System.Threading.Tasks;

namespace API.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IUsersService> _usersServiceMock;
        private Mock<AuthUtilities> _authUtilitiesMock;
        private PasswordHasherUtil _passwordHasherUtil;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _usersServiceMock = new Mock<IUsersService>();
            _authUtilitiesMock = new Mock<AuthUtilities> { CallBase = true };
            _passwordHasherUtil = new PasswordHasherUtil("your-secret-key");
            _controller = new AuthController(
                _authUtilitiesMock.Object,
                _usersServiceMock.Object,
                _passwordHasherUtil
            );
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public async Task Login_ShouldThrowUnauthorized_WhenUserNotFound()
        {
            var userLoginDto = new UserLoginDto { Username = "test", Password = "password" };

            _usersServiceMock
                .Setup(us => us.GetByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((UserReturnDto)null);

            var ex = Assert.ThrowsAsync<UnauthorizedException>(async () => await _controller.Login(userLoginDto));
            Assert.AreEqual("Invalid username or password", ex.Message);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnOk_WhenPasswordChanged()
        {
            var changePasswordDto = new ChangePasswordDto { NewPassword = "newpassword" };
            var user = new UserReturnDto
            {
                Id = 1,
                Username = "test",
                Email = "test@example.com",
                Streak = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

            _usersServiceMock
                .Setup(us => us.ChangePassword(It.IsAny<int>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(user);

            var result = await _controller.ChangePassword(changePasswordDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var userReturn = okResult.Value as UserReturnDto;
            Assert.IsNotNull(userReturn);
            Assert.AreEqual("test", userReturn.Username);
        }

        [Test]
        public async Task Get_ShouldReturnOk_WhenUserFound()
        {
            var user = new UserReturnDto
            {
                Id = 1,
                Username = "test",
                Email = "test@example.com",
                Streak = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

            _usersServiceMock
                .Setup(us => us.GetById(It.IsAny<int>()))
                .ReturnsAsync(user);

            var result = await _controller.Get();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var userReturn = okResult.Value as UserReturnDto;
            Assert.IsNotNull(userReturn);
            Assert.AreEqual("test", userReturn.Username);
        }

        [Test]
        public async Task Get_ShouldReturnUnauthorized_WhenUserIdNotFoundInClaims()
        {
            var claimsIdentity = new ClaimsIdentity();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

            var result = await _controller.Get();

            var unauthorizedResult = result as UnauthorizedResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }
    }
}