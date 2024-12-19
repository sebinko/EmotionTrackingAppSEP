using DTO;
using DTO;
using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.Services.Interfaces;

namespace Protobuf.Services;

public class ReactionService : IReactionService
{
  public async Task<ReactionDto> Create(ReactionCreateDto reactionCreateDTO, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Reactions.ReactionService.ReactionServiceClient(channel);

    var reply = await client.CreateAsync(new Reactions.ReactionCreateMessage()
    {
      Emoji = reactionCreateDTO.Emoji,
      UserId = userId,
      EmotionCheckInId = reactionCreateDTO.EmotionCheckInId
    });

    return new ReactionDto
    {
      Emoji = reply.Emoji,
      UserId = Convert.ToInt32(reply.UserId),
      EmotionCheckInId = Convert.ToInt32(reply.EmotionCheckInId),
      CreatedAt = DateTime.Parse(reply.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(reply.UpdatedAt).ToString()
    };
  }

  public async Task Delete(ReactionDeleteDto reactionDeleteDTO, int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Reactions.ReactionService.ReactionServiceClient(channel);

    await client.DeleteAsync(new Reactions.ReactionDeleteMessage()
    {
      EmotionCheckInId = reactionDeleteDTO.EmotionCheckInId,
      UserId = userId
    });
  }

  public async Task<List<ReactionDto>> GetByReactionsByEmotionCheckIn(int emotionCheckInId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Reactions.ReactionService.ReactionServiceClient(channel);

    var reply = await client.GetForReactionsCheckInAsync(new Reactions.EmotionCheckInIdMessage()
    {
      Id = emotionCheckInId
    });

    return reply.Reactions.Select(r => new ReactionDto()
    {
      Emoji = r.Emoji,
      UserId = Convert.ToInt32(r.UserId),
      EmotionCheckInId = Convert.ToInt32(r.EmotionCheckInId),
      CreatedAt = DateTime.Parse(r.CreatedAt).ToString(),
      UpdatedAt = DateTime.Parse(r.UpdatedAt).ToString()
    }).ToList();
    
  }
}