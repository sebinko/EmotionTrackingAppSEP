using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IEmotionCheckInService
{
  Task<List<EmotionCheckInDTO>> GetAll(string token);
  Task<EmotionCheckInDTO> Get(int id);
  Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn, string token);
  Task<EmotionCheckInDTO> Update(int id, EmotionCheckInDTO emotionCheckIn);
  Task<EmotionCheckInDTO> Delete(int id);
}