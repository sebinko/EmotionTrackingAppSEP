using System.Text;
using System.Text.Json;
using DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;

namespace Frontend.Services;

public class ReactionService(AuthedClient httpClient) : IReactionService
{
  public async Task<ReactionDto> Create(ReactionCreateDto reactionCreateDTO)
  {
    var json = JsonSerializer.Serialize(reactionCreateDTO);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("Reactions", content);

    return await new ApiParsingUtils<ReactionDto>().Process(response);
  }

  public async Task Delete(ReactionDeleteDto reactionCreateDTO)
  {
    var response = await httpClient.DeleteAsync($"Reactions/{reactionCreateDTO.EmotionCheckInId}");

    await new ApiParsingUtils().Process(response);
  }

  public async Task<List<ReactionDto>> GetByReactionsByEmotionCheckIn(int emotionCheckInId)
  {
    var response = await httpClient.GetAsync($"EmotionCheckIns/{emotionCheckInId}/reactions");

    return await new ApiParsingUtils<List<ReactionDto>>().Process(response);
  }
}