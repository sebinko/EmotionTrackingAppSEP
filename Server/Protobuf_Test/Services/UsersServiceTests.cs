using System;
using System.Threading.Tasks;
using DTO;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.Services;
using Protobuf.Users;

namespace Protobuf_Test.Services
{
    [TestFixture]
    public class UsersServiceTests
    {
        private Mock<Protobuf.Users.UsersService.UsersServiceClient> _mockClient;
        private Protobuf.Services.UsersService _usersService;

        [SetUp]
        public void SetUp()
        {
            _mockClient = new Mock<Protobuf.Users.UsersService.UsersServiceClient>();
            _usersService = new Protobuf.Services.UsersService();
        }

        [Test]
        public async Task Create_ReturnsUserReturnDto()
        {
            var userRegisterDto = new UserRegisterDto { Username = "testuser", Password = "password", Email = "test@example.com" };
            var userMessage = new User { Id = 1, Username = "testuser", Email = "test@example.com", Streak = 0, CreatedAt = "2023-01-01", UpdatedAt = "2023-01-01" };

            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(userMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.CreateAsync(It.IsAny<UserCreate>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _usersService.Create(userRegisterDto);

            Assert.NotNull(result);
            Assert.AreEqual(userMessage.Id, result.Id);
            Assert.AreEqual(userMessage.Username, result.Username);
            Assert.AreEqual(userMessage.Email, result.Email);
        }

        [Test]
        public async Task GetByUsernameAndPassword_ReturnsUserReturnDto()
        {
            var username = "testuser";
            var password = "password";
            var userMessage = new User { Id = 1, Username = "testuser", Email = "test@example.com", Streak = 0, CreatedAt = "2023-01-01", UpdatedAt = "2023-01-01" };

            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(userMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetByUsernameAndPasswordAsync(It.IsAny<UsernameAndPassword>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _usersService.GetByUsernameAndPassword(username, password);

            Assert.NotNull(result);
            Assert.AreEqual(userMessage.Id, result.Id);
            Assert.AreEqual(userMessage.Username, result.Username);
            Assert.AreEqual(userMessage.Email, result.Email);
        }

        [Test]
        public async Task GetByUsername_ReturnsUserReturnDto()
        {
            var username = "testuser";
            var userMessage = new User { Id = 1, Username = "testuser", Email = "test@example.com", Streak = 0, CreatedAt = "2023-01-01", UpdatedAt = "2023-01-01" };

            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(userMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetByUsernameAsync(It.IsAny<Username>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _usersService.GetByUsername(username);

            Assert.NotNull(result);
            Assert.AreEqual(userMessage.Id, result.Id);
            Assert.AreEqual(userMessage.Username, result.Username);
            Assert.AreEqual(userMessage.Email, result.Email);
        }

        [Test]
        public async Task ChangePassword_ReturnsUserReturnDto()
        {
            var userId = 1;
            var changePasswordDto = new ChangePasswordDto { NewPassword = "newpassword" };
            var userMessage = new User { Id = 1, Username = "testuser", Email = "test@example.com", Streak = 0, CreatedAt = "2023-01-01", UpdatedAt = "2023-01-01" };

            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(userMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetByIdAsync(It.IsAny<UserId>(), null, null, default))
                .Returns(asyncUnaryCall);
            _mockClient.Setup(client => client.UpdateAsync(It.IsAny<UserUpdate>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _usersService.ChangePassword(userId, changePasswordDto);

            Assert.NotNull(result);
            Assert.AreEqual(userMessage.Id, result.Id);
            Assert.AreEqual(userMessage.Username, result.Username);
            Assert.AreEqual(userMessage.Email, result.Email);
        }

        [Test]
        public async Task GetById_ReturnsUserReturnDto()
        {
            var userId = 1;
            var userMessage = new User { Id = 1, Username = "testuser", Email = "test@example.com", Streak = 0, CreatedAt = "2023-01-01", UpdatedAt = "2023-01-01" };

            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(userMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetByIdAsync(It.IsAny<UserId>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _usersService.GetById(userId);

            Assert.NotNull(result);
            Assert.AreEqual(userMessage.Id, result.Id);
            Assert.AreEqual(userMessage.Username, result.Username);
            Assert.AreEqual(userMessage.Email, result.Email);
        }
    }
}