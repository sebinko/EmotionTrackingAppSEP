using System.ComponentModel;
using API.DTO;
using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class EmotionCheckInService
{
  public async Task<List<EmotionCheckInDTO>> GetAll(int userId)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

      var reply = await client.GetAllAsync(new EmotionCheckIns.GetAllEmotionCheckInsMessage()
      {
        UserId = userId
      });

      return reply.EmotionCheckIns.Select(emotionCheckIn => new EmotionCheckInDTO
      {
        UserId = Convert.ToInt32(emotionCheckIn.UserId),
        Emotion = emotionCheckIn.Emotion,
        Description = emotionCheckIn.Description,
        CreatedAt = DateTime.Parse(emotionCheckIn.CreatedAt).ToString(),
        UpdatedAt = DateTime.Parse(emotionCheckIn.UpdatedAt).ToString(),
        Id = Convert.ToInt32(emotionCheckIn.Id)
      }).ToList();
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error getting all emotion check-ins: {e.Message}");
    }
  }
  
  public async Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn, int userId)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

      var reply = await client.CreateAsync(new EmotionCheckIns.EmotionCheckInCreateMessage()
      {
        UserId = userId,
        Emotion = emotionCheckIn.Emotion,
        Description = emotionCheckIn.Description??"",
        NewTags = { emotionCheckIn.Tags }
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
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating emotion check-in: {e.Message}");
    }
  }

  public async Task<EmotionCheckInDTO> Update(EmotionCheckInUpdateDTO emotionCheckIn, int userId)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

      var reply = await client.UpdateAsync(new EmotionCheckIns.EmotionCheckInUpdateMessage()
      {
        Emotion = emotionCheckIn.Emotion,
        Id = emotionCheckIn.id,
        UserId = userId,
        Description = emotionCheckIn.Description
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
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating emotion check-in: {e.Message}");
    }
  }

  public async Task<EmotionCheckInDTO> Delete(int id)
  {
    try
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
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating emotion check-in: {e.Message}");
    }
  }
}