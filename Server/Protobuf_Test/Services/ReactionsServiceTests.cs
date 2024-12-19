using System;
using System.Threading.Tasks;
using DTO;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.Reactions;
using Protobuf.Services;

namespace Protobuf_Test.Services
{
    [TestFixture]
    public class ReactionServiceTests
    {
        private Mock<Protobuf.Reactions.ReactionService.ReactionServiceClient> _mockClient;
        private Protobuf.Services.ReactionService _reactionService;

        [SetUp]
        public void SetUp()
        {
            _mockClient = new Mock<Protobuf.Reactions.ReactionService.ReactionServiceClient>();
            _reactionService = new Protobuf.Services.ReactionService();
        }

        [Test]
        public async Task Create_ReturnsReactionDto()
        {
            var reactionCreateDto = new ReactionCreateDto { Emoji = "👍", EmotionCheckInId = 1 };
            var userId = 1;
            var reactionMessage = new ReactionMessage
            {
                Emoji = "👍",
                UserId = userId,
                EmotionCheckInId = reactionCreateDto.EmotionCheckInId,
                CreatedAt = DateTime.Now.ToString(),
                UpdatedAt = DateTime.Now.ToString()
            };

            var asyncUnaryCall = new AsyncUnaryCall<ReactionMessage>(
                Task.FromResult(reactionMessage),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.CreateAsync(It.IsAny<ReactionCreateMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            var result = await _reactionService.Create(reactionCreateDto, userId);

            Assert.NotNull(result);
            Assert.AreEqual("👍", result.Emoji);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(reactionCreateDto.EmotionCheckInId, result.EmotionCheckInId);
        }

        [Test]
        public async Task Delete_CallsDeleteAsync()
        {
            var reactionDeleteDto = new ReactionDeleteDto { EmotionCheckInId = 1 };
            var userId = 1;

            var asyncUnaryCall = new AsyncUnaryCall<ReactionMessage>(
                Task.FromResult(new ReactionMessage()),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );

            _mockClient.Setup(client => client.DeleteAsync(It.IsAny<ReactionDeleteMessage>(), null, null, default))
                .Returns(asyncUnaryCall);

            await _reactionService.Delete(reactionDeleteDto, userId);

            _mockClient.Verify(client => client.DeleteAsync(It.IsAny<ReactionDeleteMessage>(), null, null, default), Times.Once);
        }
    }
}