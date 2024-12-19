using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UserFriendsServiceTests
    {
        private Mock<Protobuf.Users.UserFriendsService.UserFriendsServiceClient> _mockClient;
        private Protobuf.Services.UserFriendsService _userFriendsService;

        [SetUp]
        public void SetUp()
        {
            _mockClient = new Mock<Protobuf.Users.UserFriendsService.UserFriendsServiceClient>();
            _userFriendsService = new Protobuf.Services.UserFriendsService();
        }

        [Test]
        public async Task CreateFriendship_CreatesNewFriendship()
        {
            var user1Id = 1;
            var user2Id = 2;
            var existingFriendship = new FriendshipMessage { User1Id = 0, User2Id = 0, IsAccepted = false };

            var asyncUnaryCall = new AsyncUnaryCall<FriendshipMessage>(
                Task.FromResult(existingFriendship),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetFriendshipAsync(It.IsAny<FriendshipSimpleMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            await _userFriendsService.CreateFriendship(user1Id, user2Id);

            _mockClient.Verify(client => client.CreateFriendshipAsync(It.IsAny<FriendshipSimpleMessage>(), null, null, default), Times.Once);
        }

        [Test]
        public async Task RemoveFriendship_RemovesExistingFriendship()
        {
            var user1Id = 1;
            var user2Id = 2;

            var asyncUnaryCall = new AsyncUnaryCall<FriendshipMessage>(
                Task.FromResult(new FriendshipMessage()),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.RemoveFriendshipAsync(It.IsAny<FriendshipSimpleMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            await _userFriendsService.RemoveFriendship(user1Id, user2Id);

            _mockClient.Verify(client => client.RemoveFriendshipAsync(It.IsAny<FriendshipSimpleMessage>(), null, null, default), Times.Once);
        }

        [Test]
        public async Task GetFriends_ReturnsListOfFriends()
        {
            var userId = 1;
            var friendsList = new FriendsWithCheckIns
            {
                Friends = { new FriendWithCheckIn { Friend = new User { Id = 1, Username = "user1", Email = "user1@example.com", CreatedAt = "2023-01-01", UpdatedAt = "2023-01-02", Streak = 5 }, Emotion = "Happy" } }
            };

            var asyncUnaryCall = new AsyncUnaryCall<FriendsWithCheckIns>(
                Task.FromResult(friendsList),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.GetAllFriendsAsync(It.IsAny<UserId>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _userFriendsService.GetFriends(userId);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("user1", result[0].User.Username);
            Assert.AreEqual("Happy", result[0].LatestCheckIn);
        }
    }
}