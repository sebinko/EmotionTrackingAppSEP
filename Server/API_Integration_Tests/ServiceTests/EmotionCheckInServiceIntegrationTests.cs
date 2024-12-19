using DTO;
using Grpc.Core;
using Protobuf.Services;

namespace API_Integration_Tests;

public class EmotionCheckInServiceIntegrationTests
{
  private readonly EmotionCheckInService service;

    public EmotionCheckInServiceIntegrationTests()
    {
        service = new EmotionCheckInService();
    }

    [Fact]
    public async Task GetById_ShouldReturnEmotionCheckIn()
    {
        int id = 1;
        int userId = 1;

        var result = await service.GetById(id, userId);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public async Task GetById_ShouldThrowRpcException_WhenIdDoesNotExist()
    {
      int id = -1; // (ID does not exist)
      int userId = 1;

      var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.GetById(id, userId));

      Assert.Equal(StatusCode.Internal, exception.StatusCode);
      Assert.Contains("Cannot invoke", exception.Status.Detail);
    }

    [Fact]
    public async Task GetByTags_ShouldReturnEmotionCheckIns()
    {
        var tags = new List<TagDto>
        {
            new TagDto { Key = "1", Type = TagType.WHAT }
        };
        int userId = 1;

        var result = await service.GetByTags(tags, userId);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task GetByTags_ShouldReturnEmptyList_WhenTagsDoNotMatch()
    {
      var tags = new List<TagDto>
      {
        new TagDto { Key = "nonexistent", Type = TagType.WHAT }
      };
      int userId = 1;

      var result = await service.GetByTags(tags, userId);

      Assert.NotNull(result);
      Assert.Empty(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllEmotionCheckIns()
    {
        int userId = 1;

        var result = await service.GetAll(userId);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Create_ShouldCreateEmotionCheckIn()
    {
        var emotionCheckIn = new EmotionCheckInCreateDto
        {
            Emotion = "happy",
            Description = "Feeling great!",
            Tags = new List<TagDto>
            {
                new TagDto { Key = "happy", Type = TagType.WHAT }
            }
        };
        int userId = 1;

        var result = await service.Create(emotionCheckIn, userId);

        Assert.NotNull(result);
        Assert.Equal(emotionCheckIn.Emotion, result.Emotion);
    }
    
    [Fact]
    public async Task Create_ShouldThrowException_WhenEmotionIsNull()
    {
      var emotionCheckIn = new EmotionCheckInCreateDto
      {
        Emotion = null,
        Description = "Feeling great!",
        Tags = new List<TagDto>
        {
          new TagDto { Key = "happy", Type = TagType.WHAT }
        }
      };
      int userId = 1;

      await Assert.ThrowsAsync<ArgumentNullException>(() => service.Create(emotionCheckIn, userId));
    }

    [Fact]
    public async Task Update_ShouldUpdateEmotionCheckIn()
    {
        var emotionCheckIn = new EmotionCheckInUpdateDto
        {
            Id = 1,
            Emotion = "sad",
            Description = "Feeling down.",
            Tags = new List<TagDto>
            {
                new TagDto { Key = "sad", Type = TagType.WHAT }
            }
        };
        int userId = 1;

        var result = await service.Update(emotionCheckIn, userId);

        Assert.NotNull(result);
        Assert.Equal(emotionCheckIn.Emotion, result.Emotion);
    }
    
    [Fact]
    public async Task Update_ShouldThrowRpcException_WhenIdDoesNotExist()
    {
      var emotionCheckIn = new EmotionCheckInUpdateDto
      {
        Id = -1, // (ID does not exist)
        Emotion = "sad",
        Description = "Feeling down.",
        Tags = new List<TagDto>
        {
          new TagDto { Key = "sad", Type = TagType.WHAT }
        }
      };
      int userId = 1;

      var exception = await Assert.ThrowsAsync<Grpc.Core.RpcException>(() => service.Update(emotionCheckIn, userId));

      Assert.Equal(StatusCode.Internal, exception.StatusCode);
      Assert.Contains("Cannot invoke", exception.Status.Detail);
    }

    [Fact]
    public async Task Delete_ShouldDeleteEmotionCheckIn()
    {
        int id = 1;

        var result = await service.Delete(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public async Task Delete_ShouldThrowRpcException_WhenIdDoesNotExist()
    {
      int id = -1; // id does not exist

      var exception = await Assert.ThrowsAsync<RpcException>(() => service.Delete(id));

      Assert.Equal(StatusCode.Internal, exception.StatusCode);
    }
}