using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IEmotionsService
{
  Task<List<EmotionDTO>?> GetAll(string? emotionQuery, string? emotionColor);
  Task<EmotionCheckInDTO> GetById(int id);
  Task<List<EmotionDTO>> GetEmotionByColor(string color);
}