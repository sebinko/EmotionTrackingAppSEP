using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IEmotionsService
{
  Task<List<EmotionDTO>?> GetAll(string? emotionQuery, string? emotionColor);
}