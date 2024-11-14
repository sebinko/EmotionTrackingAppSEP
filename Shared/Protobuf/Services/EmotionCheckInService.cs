using API.DTO;
using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class EmotionCheckInService
{
  public async Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new EmotionCheckIns.EmotionCheckInsService.EmotionCheckInsServiceClient(channel);

      var reply = await client.CreateAsync(new EmotionCheckIns.EmotionCheckInCreate()
      {
        EmotionToUpdate = new EmotionCheckIns.EmotionCheckIn()
        {
          UserId = emotionCheckIn.UserId.ToString(),
          Emotion = emotionCheckIn.Emotion,
        },
        NewTags = { }
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