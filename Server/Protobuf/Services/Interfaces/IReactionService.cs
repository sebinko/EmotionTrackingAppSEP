using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IReactionService
  {
    Task<ReactionDto> Create(ReactionCreateDto reactionCreateDTO, int userId);
  }
}