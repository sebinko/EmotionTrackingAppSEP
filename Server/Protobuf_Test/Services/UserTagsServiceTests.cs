using System.Collections.Generic;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using Protobuf.Users;

namespace Protobuf_Test.Services
{
  [TestFixture]
  public class UserTagsServiceTests
  {
    private Mock<Protobuf.Users.UserTagsService.UserTagsServiceClient> _mockClient;
    private Protobuf.Services.UserTagsService _userTagsService;

    [SetUp]
    public void SetUp()
    {
      _mockClient = new Mock<Protobuf.Users.UserTagsService.UserTagsServiceClient>();
      _userTagsService = new Protobuf.Services.UserTagsService();
    }

    [Test]
    public void GetAllTags_ReturnsListOfTagDto()
    {
      var userId = 1;
      var tags = new List<Tag>
      {
        new Tag { Key = "key1", Type = Protobuf.Users.TagType.What },
        new Tag { Key = "key2", Type = Protobuf.Users.TagType.With }
      };
      var reply = new TagsList();
      reply.Tags.AddRange(tags);

      _mockClient.Setup(c => c.GetAllTags(It.IsAny<UserId>(), null, null, default))
        .Returns(reply);

      var result = _userTagsService.GetAllTags(userId);

      Assert.IsNotNull(result);
      Assert.AreEqual(2, result.Count);
      Assert.AreEqual("key1", result[0].Key);
      Assert.AreEqual(DTO.TagType.WHAT, result[0].Type);
      Assert.AreEqual("key2", result[1].Key);
      Assert.AreEqual(DTO.TagType.WITH, result[1].Type);
    }
  }
}