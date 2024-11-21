using API.DTO;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class EmotionsService
{
  public async Task<List<EmotionDTO>> GetAll(string? emotionQuery, string? emotionColor)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Emotions.EmotionsService.EmotionsServiceClient(channel);

    var request = new Emotions.EmotionsRequest();
    
    if (emotionQuery is not null)
    {
      request.Emotion = emotionQuery;
    }
    
    if (emotionColor is not null)
    {
      request.Color = emotionColor;
    }
    
    var reply = await client.GetEmotionsMethodAsync(request);
    
    return reply.Emotions.Select(emotion => new EmotionDTO
    {
      Emotion = emotion.Emotion_,
      Color = emotion.Color,
      Description = emotion.Description
    }).ToList();
  }
}