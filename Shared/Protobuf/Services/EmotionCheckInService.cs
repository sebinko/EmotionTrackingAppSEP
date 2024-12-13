using System.ComponentModel;
using API.DTO;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.EmotionCheckIns;
using ProtoTagType = Protobuf.EmotionCheckIns.TagType;
using TagType = API.DTO.TagType;

namespace Protobuf.Services;

public class EmotionCheckInService
{
  public async Task<EmotionCheckInDTO> GetById(int id, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

    var reply = await client.GetByIdAsync(new EmotionCheckIns.EmotionCheckInIdMessage()
    {
      Id = id,
      UserId = userId
    });

    return new EmotionCheckInDTO
    {
      UserId = Convert.ToInt32(reply.UserId),
      Emotion = reply.Emotion,
      Description = reply.Description,
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString(),
      Id = Convert.ToInt32(reply.Id),
      Tags = reply.Tags.Select(tag => new TagDTO
      {
        Key = tag.Key,
        Type = (TagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      }).ToList()
    };
  }
  
  public async Task<List<EmotionCheckInDTO>> GetAll(int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

    var reply = await client.GetAllAsync(new EmotionCheckIns.GetAllEmotionCheckInsMessage()
    {
      UserId = userId
    });
  
    var repl = reply.EmotionCheckIns.Select(emotionCheckIn => new EmotionCheckInDTO
    {
      UserId = Convert.ToInt32(emotionCheckIn.UserId),
      Emotion = emotionCheckIn.Emotion,
      Description = emotionCheckIn.Description,
      CreatedAt = DateTime.Parse(emotionCheckIn.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(emotionCheckIn.UpdatedAt).ToString(),
      Id = Convert.ToInt32(emotionCheckIn.Id),
      Tags = emotionCheckIn.Tags.Select(tag => new TagDTO
      {
        Key = tag.Key,
        Type = (TagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      }).ToList()
    }).ToList();

    return repl;
  }

  public async Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

    var tags = new RepeatedField<Tag>();

    foreach (var tag in emotionCheckIn.Tags)
    {
      tags.Add(new Tag
      {
        Key = tag.Key, Type = (ProtoTagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
      });
    }

    var reply = await client.CreateAsync(new EmotionCheckIns.EmotionCheckInCreateMessage()
    {
      UserId = userId,
      Emotion = emotionCheckIn.Emotion,
      Description = emotionCheckIn.Description ?? "",
      Tags = { tags }
    });

    return new EmotionCheckInDTO
    {
      UserId = Convert.ToInt32(reply.UserId),
      Emotion = reply.Emotion,
      Description = reply.Description,
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString(),
      Id = Convert.ToInt32(reply.Id)
    };
  }

  public async Task<EmotionCheckInDTO> Update(EmotionCheckInUpdateDTO emotionCheckIn, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

    var reply = await client.UpdateAsync(new EmotionCheckIns.EmotionCheckInUpdateMessage()
      {
        Emotion = emotionCheckIn.Emotion,
        Id = emotionCheckIn.id,
        UserId = userId,
        Description = emotionCheckIn.Description,
        Tags = { emotionCheckIn.Tags.Select(tag => new Tag
        {
          Key = tag.Key, Type = (ProtoTagType)Enum.Parse(typeof(TagType), tag.Type.ToString(), true)
        }) }
      }
    );

    return new EmotionCheckInDTO
    {
      UserId = Convert.ToInt32(reply.UserId),
      Emotion = reply.Emotion,
      Description = reply.Description,
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString(),
      Id = Convert.ToInt32(reply.Id)
    };
  }

  public async Task<EmotionCheckInDTO> Delete(int id)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

    var reply = await client.DeleteAsync(new EmotionCheckIns.EmotionCheckInIdMessage()
    {
      Id = id
    });

    return new EmotionCheckInDTO
    {
      UserId = Convert.ToInt32(reply.UserId),
      Emotion = reply.Emotion,
      Description = reply.Description,
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString(),
      Id = Convert.ToInt32(reply.Id)
    };
  }
}