using API.DTO;
using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class ReactionService
{
  public async Task<ReactionDTO> Create(ReactionCreateDTO reactionCreateDTO, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Reactions.ReactionService.ReactionServiceClient(channel);

    var reply = await client.CreateAsync(new Reactions.ReactionCreateMessage()
    {
      Emoji = reactionCreateDTO.Emoji,
      UserId = userId,
      EmotionCheckInId = reactionCreateDTO.EmotionCheckInId
    });

    return new ReactionDTO
    {
      Emoji = reply.Emoji,
      UserId = Convert.ToInt32(reply.UserId),
      EmotionCheckInId = Convert.ToInt32(reply.EmotionCheckInId),
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString()
    };
  }
}