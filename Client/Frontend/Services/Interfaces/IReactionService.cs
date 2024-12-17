using DTO;

namespace Frontend.Services.Interfaces;

public interface IReactionService
{
  Task<ReactionDto> Create(ReactionCreateDto reactionCreateDTO);
  Task Delete(ReactionDeleteDto reactionCreateDTO);
  Task<List<ReactionDto>> GetByReactionsByEmotionCheckIn(int emotionCheckInId);
}