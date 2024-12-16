using DTO;

namespace Frontend.Services.Interfaces;

public interface IEmotionCheckInService
{
  Task<List<EmotionCheckInDto>?> GetAll();
  Task<List<EmotionCheckInDto>?> GetByTags(GetEmotionCheckInByTagsDto getEmotionCheckInByTags);
  Task<EmotionCheckInDto?> Get(int id);
  Task<EmotionCheckInDto?> Create(EmotionCheckInCreateDto emotionCheckIn);
  Task<EmotionCheckInDto?> Update(int id, EmotionCheckInUpdateDto emotionCheckIn);
  Task<EmotionCheckInDto?> Delete(int id);
}