

using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IEmotionCheckInService
  {
    Task<EmotionCheckInDto> GetById(int id, int userId);
    Task<List<EmotionCheckInDto>> GetAll(int userId);
    Task<EmotionCheckInDto> Create(EmotionCheckInCreateDto emotionCheckIn, int userId);
    Task<EmotionCheckInDto> Update(EmotionCheckInUpdateDto emotionCheckIn, int userId);
    Task<EmotionCheckInDto> Delete(int id);
  }
}