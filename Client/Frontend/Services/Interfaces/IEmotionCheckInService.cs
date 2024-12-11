using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IEmotionCheckInService
{
  Task<List<EmotionCheckInDTO>?> GetAll();
  Task<EmotionCheckInDTO?> Get(int id);
  Task<EmotionCheckInDTO?> Create(EmotionCheckInCreateDTO emotionCheckIn);
  Task<EmotionCheckInDTO?> Update(int id, EmotionCheckInUpdateDTO emotionCheckIn);
  Task<EmotionCheckInDTO?> Delete(int id);
}