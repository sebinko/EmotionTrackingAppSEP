using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.EmotionCheckIns;
using Protobuf.Users;
using Protobuf.Services;

namespace Protobuf_Test.Services
{
    [TestFixture]
    public class EmotionCheckInServiceTests
    {
        private Mock<EmotionCheckInsService.EmotionCheckInsServiceClient> _mockEmotionCheckInsServiceClient;
        private Mock<Protobuf.Users.UsersService.UsersServiceClient> _mockUsersServiceClient;
        private EmotionCheckInService _service;

        [SetUp]
        public void SetUp()
        {
            _mockEmotionCheckInsServiceClient = new Mock<EmotionCheckInsService.EmotionCheckInsServiceClient>();
            _mockUsersServiceClient = new Mock<Protobuf.Users.UsersService.UsersServiceClient>();
            _service = new EmotionCheckInService();
        }

        [Test]
        public async Task GetById_UserNotFound_ThrowsRpcException()
        {
            var asyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult<User>(null),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockUsersServiceClient.Setup(client => client.GetByIdAsync(It.IsAny<UserId>(), null, null, default))
                .Returns(asyncUnaryCall);

            var ex = Assert.ThrowsAsync<RpcException>(() => _service.GetById(1, 1));
            Assert.AreEqual(StatusCode.NotFound, ex.StatusCode);
            Assert.AreEqual("User not found", ex.Status.Detail);
        }

        [Test]
        public async Task GetById_ReturnsEmotionCheckInDto()
        {
            var user = new User { Id = 1 };
            var emotionCheckInMessage = new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" };

            var userAsyncUnaryCall = new AsyncUnaryCall<User>(
                Task.FromResult(user),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            var emotionCheckInAsyncUnaryCall = new AsyncUnaryCall<EmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockUsersServiceClient.Setup(client => client.GetByIdAsync(It.IsAny<UserId>(), null, null, default))
                .Returns(userAsyncUnaryCall);
            _mockEmotionCheckInsServiceClient.Setup(client => client.GetByIdAsync(It.IsAny<EmotionCheckInIdMessage>(), null, null, default))
                .Returns(emotionCheckInAsyncUnaryCall);

            var result = await _service.GetById(1, 1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Happy", result.Emotion);
            Assert.AreEqual("Feeling great", result.Description);
        }

        [Test]
        public async Task GetByTags_ReturnsListOfEmotionCheckInDto()
        {
            var tags = new List<TagDto> { new TagDto { Key = "key1", Type = DTO.TagType.WHAT } };
            var emotionCheckInMessages = new ListEmotionCheckInMessage
            {
                EmotionCheckIns = { new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" } }
            };

            var asyncUnaryCall = new AsyncUnaryCall<ListEmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessages),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockEmotionCheckInsServiceClient.Setup(client => client.GetAllByTagAsync(It.IsAny<TagFilter>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _service.GetByTags(tags, 1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Feeling great", result[0].Description);
        }

        [Test]
        public async Task GetAll_ReturnsListOfEmotionCheckInDto()
        {
            var emotionCheckInMessages = new ListEmotionCheckInMessage
            {
                EmotionCheckIns = { new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" } }
            };

            var asyncUnaryCall = new AsyncUnaryCall<ListEmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessages),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockEmotionCheckInsServiceClient.Setup(client => client.GetAllAsync(It.IsAny<GetAllEmotionCheckInsMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _service.GetAll(1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Feeling great", result[0].Description);
        }

        [Test]
        public async Task Create_ReturnsEmotionCheckInDto()
        {
            var emotionCheckInCreateDto = new EmotionCheckInCreateDto { Emotion = "Happy", Tags = new List<TagDto> { new TagDto { Key = "key1", Type = DTO.TagType.WHAT } } };
            var emotionCheckInMessage = new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" };

            var asyncUnaryCall = new AsyncUnaryCall<EmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockEmotionCheckInsServiceClient.Setup(client => client.CreateAsync(It.IsAny<EmotionCheckInCreateMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _service.Create(emotionCheckInCreateDto, 1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Happy", result.Emotion);
            Assert.AreEqual("Feeling great", result.Description);
        }

        [Test]
        public async Task Update_ReturnsEmotionCheckInDto()
        {
            var emotionCheckInUpdateDto = new EmotionCheckInUpdateDto { Id = 1, Emotion = "Happy", Tags = new List<TagDto> { new TagDto { Key = "key1", Type = DTO.TagType.WHAT } } };
            var emotionCheckInMessage = new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" };

            var asyncUnaryCall = new AsyncUnaryCall<EmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockEmotionCheckInsServiceClient.Setup(client => client.UpdateAsync(It.IsAny<EmotionCheckInUpdateMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _service.Update(emotionCheckInUpdateDto, 1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Happy", result.Emotion);
            Assert.AreEqual("Feeling great", result.Description);
        }

        [Test]
        public async Task Delete_ReturnsEmotionCheckInDto()
        {
            var emotionCheckInMessage = new EmotionCheckInMessage { Id = 1, UserId = 1, Emotion = "Happy", Description = "Feeling great" };

            var asyncUnaryCall = new AsyncUnaryCall<EmotionCheckInMessage>(
                Task.FromResult(emotionCheckInMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockEmotionCheckInsServiceClient.Setup(client => client.DeleteAsync(It.IsAny<EmotionCheckInIdMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _service.Delete(1);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Happy", result.Emotion);
            Assert.AreEqual("Feeling great", result.Description);
        }
    }
}