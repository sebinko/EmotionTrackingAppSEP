using System.Text;
using System.Text.Json;
using DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using SharedUtil;

namespace Frontend.Services;

public class EmotionCheckInService(AuthedClient httpClient) : IEmotionCheckInService
{
  public async Task<List<EmotionCheckInDto>?> GetAll()
  {
    var response = await httpClient.GetAsync("EmotionCheckIns");

    return await new ApiParsingUtils<List<EmotionCheckInDto>>().Process(response);
  }

  public async Task<EmotionCheckInDto?> Get(int id)
  {
    var response = await httpClient.GetAsync($"EmotionCheckIns/{id}");

    return await new ApiParsingUtils<EmotionCheckInDto>().Process(response);
  }

  public async Task<EmotionCheckInDto?> Create(EmotionCheckInCreateDto emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("EmotionCheckIns", content);


    return await new ApiParsingUtils<EmotionCheckInDto>().Process(response);
  }

  public async Task<EmotionCheckInDto?> Update(int id, EmotionCheckInUpdateDto emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PatchAsync($"EmotionCheckIns", content);

    return await new ApiParsingUtils<EmotionCheckInDto>().Process(response);
  }


  public async Task<EmotionCheckInDto?> Delete(int id)
  {
    var response = await httpClient.DeleteAsync($"EmotionCheckIns/{id}");

    return await new ApiParsingUtils<EmotionCheckInDto>().Process(response);
  }
}