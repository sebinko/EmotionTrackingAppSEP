using API.DTO;
using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class EmotionCheckInService
{
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
        NewTags = { emotionCheckIn.Tags }
      });

      return new EmotionCheckInDTO
      {
        UserId = Convert.ToInt32(reply.UserId),
        Emotion = reply.Emotion,
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
      }
      );

      return new EmotionCheckInDTO
      {
        UserId = Convert.ToInt32(reply.UserId),
        Emotion = reply.Emotion,
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