using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.Emotions;

namespace Protobuf.Services.Tests
{
    [TestFixture]
    public class EmotionsServiceTests
    {
        private Mock<Emotions.EmotionsService.EmotionsServiceClient> _mockEmotionsServiceClient;
        private EmotionsService _service;

        [SetUp]
        public void SetUp()
        {
            _mockEmotionsServiceClient = new Mock<Emotions.EmotionsService.EmotionsServiceClient>();
            _service = new EmotionsService();
        }

        [Test]
        public async Task GetAll_WithQueryAndColor_ReturnsFilteredEmotions()
        {
            var emotionQuery = "Happy";
            var emotionColor = "Yellow";
            var mockReply = new EmotionsMessage
            {
                Emotions =
                {
                    new Emotion { Emotion_ = "Happy", Color = "Yellow", Description = "Feeling great" }
                }
            };
            var mockReplyCall = new AsyncUnaryCall<EmotionsMessage>(
                Task.FromResult(mockReply),
                Task.FromResult(new Metadata()),
                () => new Grpc.Core.Status(StatusCode.OK, string.Empty),
                () => new Metadata(),
                () => { }
            );

            _mockEmotionsServiceClient
                .Setup(client => client.GetEmotionsMethodAsync(It.IsAny<EmotionsRequest>(), null, null, default))
                .Returns(mockReplyCall);

            var result = await _service.GetAll(emotionQuery, emotionColor);

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Yellow", result[0].Color);
            Assert.AreEqual("Feeling great", result[0].Description);
        }

        [Test]
        public async Task GetAll_WithoutQueryAndColor_ReturnsAllEmotions()
        {
            var mockReply = new EmotionsMessage
            {
                Emotions =
                {
                    new Emotion { Emotion_ = "Happy", Color = "Yellow", Description = "Feeling great" },
                    new Emotion { Emotion_ = "Sad", Color = "Blue", Description = "Feeling down" }
                }
            };
            var mockReplyCall = new AsyncUnaryCall<EmotionsMessage>(
                Task.FromResult(mockReply),
                Task.FromResult(new Metadata()),
                () => new Grpc.Core.Status(StatusCode.OK, string.Empty),
                () => new Metadata(),
                () => { }
            );

            _mockEmotionsServiceClient
                .Setup(client => client.GetEmotionsMethodAsync(It.IsAny<EmotionsRequest>(), null, null, default))
                .Returns(mockReplyCall);

            var result = await _service.GetAll(null, null);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Happy", result[0].Emotion);
            Assert.AreEqual("Yellow", result[0].Color);
            Assert.AreEqual("Feeling great", result[0].Description);
            Assert.AreEqual("Sad", result[1].Emotion);
            Assert.AreEqual("Blue", result[1].Color);
            Assert.AreEqual("Feeling down", result[1].Description);
        }
    }
}