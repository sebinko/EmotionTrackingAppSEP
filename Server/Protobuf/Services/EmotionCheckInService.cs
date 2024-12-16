using DTO;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.EmotionCheckIns;
using Protobuf.Services.Interfaces;
using ProtoTagType = Protobuf.EmotionCheckIns.TagType;
using TagType = DTO.TagType;

namespace Protobuf.Services;

public class EmotionCheckInService : IEmotionCheckInService
{
  private readonly EmotionCheckInsService.EmotionCheckInsServiceClient
    _emotionCheckInsServiceClient;

  private readonly Users.UsersService.UsersServiceClient _usersServiceClient;
  private readonly GrpcChannel _channel;

  public EmotionCheckInService()
  {
    _channel = GrpcChannel.ForAddress("http://localhost:8888");
    _emotionCheckInsServiceClient =
      new EmotionCheckInsService.EmotionCheckInsServiceClient(_channel);
    _usersServiceClient = new Users.UsersService.UsersServiceClient(_channel);
  }

  public async Task<EmotionCheckInDto> GetById(int id, int userId)
  {
    var user = await _usersServiceClient.GetByIdAsync(new Users.UserId { Id = userId });

    if (user == null)
    {
      throw new RpcException(new Grpc.Core.Status(StatusCode.NotFound, "User not found"));
    }

    var reply = await _emotionCheckInsServiceClient.GetByIdAsync(new EmotionCheckInIdMessage
      { Id = id, UserId = userId });

    return MapToEmotionCheckInDto(reply);
  }

  public async Task<List<EmotionCheckInDto>> GetByTags(List<TagDto> tags, int userId)
  {
    var tagFilter = new TagFilter();
    
    foreach (var tag in tags)
    {
      tagFilter.Filters.Add(new TagFilterSingle()
      {
        Key = tag.Key,
        Type = tag.Type.ToString(),
        UserId = userId,
      });
    }

    var reply = await _emotionCheckInsServiceClient.GetAllByTagAsync(tagFilter);
    
    return reply.EmotionCheckIns.Select(MapToEmotionCheckInDto).ToList();
  }

  public async Task<List<EmotionCheckInDto>> GetAll(int userId)
  {
    var reply = await _emotionCheckInsServiceClient.GetAllAsync(new GetAllEmotionCheckInsMessage
      { UserId = userId });
    return reply.EmotionCheckIns.Select(MapToEmotionCheckInDto).ToList();
  }

  public async Task<EmotionCheckInDto> Create(EmotionCheckInCreateDto emotionCheckIn, int userId)
  {
    var tags = new RepeatedField<Tag>();
    foreach (var tag in emotionCheckIn.Tags)
    {
      tags.Add(new Tag
      {
        Key = tag.Key, Type = (ProtoTagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      });
    }

    var reply = await _emotionCheckInsServiceClient.CreateAsync(new EmotionCheckInCreateMessage
    {
      UserId = userId,
      Emotion = emotionCheckIn.Emotion,
      Description = emotionCheckIn.Description ?? "",
      Tags = { tags }
    });

    return MapToEmotionCheckInDto(reply);
  }

  public async Task<EmotionCheckInDto> Update(EmotionCheckInUpdateDto emotionCheckIn, int userId)
  {
    var tags = new RepeatedField<Tag>();
    foreach (var tag in emotionCheckIn.Tags)
    {
      tags.Add(new Tag
      {
        Key = tag.Key, Type = (ProtoTagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      });
    }

    var reply = await _emotionCheckInsServiceClient.UpdateAsync(new EmotionCheckInUpdateMessage
    {
      Id = emotionCheckIn.Id,
      UserId = userId,
      Emotion = emotionCheckIn.Emotion,
      Description = emotionCheckIn.Description,
      Tags = { tags }
    });

    return MapToEmotionCheckInDto(reply);
  }

  public async Task<EmotionCheckInDto> Delete(int id)
  {
    var reply =
      await _emotionCheckInsServiceClient.DeleteAsync(new EmotionCheckInIdMessage { Id = id });
    return MapToEmotionCheckInDto(reply);
  }

  private static EmotionCheckInDto MapToEmotionCheckInDto(EmotionCheckInMessage reply)
  {
    return new EmotionCheckInDto
    {
      UserId = Convert.ToInt32(reply.UserId),
      Emotion = reply.Emotion,
      Description = reply.Description,
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString(),
      Id = Convert.ToInt32(reply.Id),
      Tags = reply.Tags.Select(tag => new TagDto
      {
        Key = tag.Key,
        Type = (TagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      }).ToList()
    };
  }
}