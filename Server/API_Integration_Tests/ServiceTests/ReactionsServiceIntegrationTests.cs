using DTO;
using Grpc.Core;
using Protobuf.Services;

namespace API_Integration_Tests;

public class ReactionsServiceIntegrationTests
{
  private readonly ReactionService service;

  public ReactionsServiceIntegrationTests()
  {
    service = new ReactionService();
  }

  [Fact]
  public async Task Create_ShouldCreateReaction()
  {
    var reactionCreateDto = new ReactionCreateDto
    {
      Emoji = "😊",
      EmotionCheckInId = 2
    };
    int userId = 19;

    var result = await service.Create(reactionCreateDto, userId);

    Assert.NotNull(result);
    Assert.Equal(reactionCreateDto.Emoji, result.Emoji);
    Assert.Equal(reactionCreateDto.EmotionCheckInId, result.EmotionCheckInId);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenEmojiIsNull()
  {
    var reactionCreateDto = new ReactionCreateDto
    {
      Emoji = null,
      EmotionCheckInId = 2
    };
    int userId = 19;

    await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(reactionCreateDto, userId));
  }
  
  [Fact]
  public async Task Create_ShouldThrowException_WhenEmotionCheckInIdIsInvalid()
  {
    var reactionCreateDto = new ReactionCreateDto
    {
      Emoji = "😊",
      EmotionCheckInId = -1
    };
    int userId = 19;

    var exception = await Assert.ThrowsAsync<RpcException>(() => service.Create(reactionCreateDto, userId));
    Assert.Equal(StatusCode.NotFound, exception.StatusCode);
    Assert.Equal("Foreign Entity not found.", exception.Status.Detail);
  }
  
  [Fact]
  public async Task Delete_ShouldDeleteReaction()
  {
    var reactionDeleteDto = new ReactionDeleteDto
    {
      EmotionCheckInId = 2
    };
    int userId = 19;

    await service.Delete(reactionDeleteDto, userId);
  }

  [Fact]
  public async Task GetByReactionsByEmotionCheckIn_ShouldReturnReactions()
  {
    int emotionCheckInId = 1;

    var result = await service.GetByReactionsByEmotionCheckIn(emotionCheckInId);

    Assert.NotNull(result);
    Assert.NotEmpty(result);
  }
  
  [Fact]
  public async Task GetByReactionsByEmotionCheckIn_ShouldReturnEmptyList_WhenNoReactionsExist()
  {
    int emotionCheckInId = 999; //this ID does not exist

    var result = await service.GetByReactionsByEmotionCheckIn(emotionCheckInId);

    Assert.NotNull(result);
    Assert.Empty(result);
  }
}