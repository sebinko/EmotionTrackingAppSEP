using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.EmotionCheckIns;
using EmotionCheckIn = Entities.EmotionCheckIn;

namespace Protobuf.Services;

public class EmotionCheckInService
{
    public async Task<EmotionCheckIn> Create(EmotionCheckIn emotionCheckIn)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:8888");
            var client = new EmotionCheckIns.EmotionCheckInService.EmotionCheckInServiceClient(channel);

            var reply = await client.CreateAsync(new EmotionCheckIns.EmotionCheckIn
            {
                UserId = emotionCheckIn.UserId,
                Emotion = emotionCheckIn.Emotion,
                CreatedAt = emotionCheckIn.CreatedAt,
                UpdatedAt = emotionCheckIn.UpdatedAt,
                Id = emotionCheckIn.Id
            });

            DateTime.TryParse(reply.CreatedAt, out var createdAt);
            DateTime.TryParse(reply.UpdatedAt, out var updatedAt);


            emotionCheckIn.Id = Convert.ToInt32(reply.Id);
            emotionCheckIn.UserId = reply.UserId;
            emotionCheckIn.Emotion = reply.Emotion;
            emotionCheckIn.CreatedAt = createdAt;
            emotionCheckIn.UpdatedAt = updatedAt;

            return emotionCheckIn;
        }
        catch (RpcException e)
        {
            throw new Exception($"JavaDAO: Error creating emotion check-in: {e.Message}");
        }
    }
    
}
