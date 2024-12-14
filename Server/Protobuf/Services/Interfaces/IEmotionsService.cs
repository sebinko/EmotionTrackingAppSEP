
using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IEmotionsService
  {
    Task<List<EmotionDto>> GetAll(string? emotionQuery, string? emotionColor);
  }
}