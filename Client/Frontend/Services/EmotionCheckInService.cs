using System.Text;
using System.Text.Json;
using API.DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using SharedUtil;

namespace Frontend.Services;

public class EmotionCheckInService(AuthedClient httpClient) : IEmotionCheckInService
{
  public async Task<List<EmotionCheckInDTO>?> GetAll()
  {
    var response = await httpClient.GetAsync("EmotionCheckIns");

    return await new ApiParsingUtils<List<EmotionCheckInDTO>>().Process(response);
  }

  public async Task<EmotionCheckInDTO?> Get(int id)
  {
    var response = await httpClient.GetAsync($"EmotionCheckIns/{id}");

    return await new ApiParsingUtils<EmotionCheckInDTO>().Process(response);
  }

  public async Task<EmotionCheckInDTO?> Create(EmotionCheckInCreateDTO emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("EmotionCheckIns", content);


    return await new ApiParsingUtils<EmotionCheckInDTO>().Process(response);
  }

  public async Task<EmotionCheckInDTO?> Update(int id, EmotionCheckInUpdateDTO emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PatchAsync($"EmotionCheckIns", content);

    return await new ApiParsingUtils<EmotionCheckInDTO>().Process(response);
  }


  public async Task<EmotionCheckInDTO?> Delete(int id)
  {
    var response = await httpClient.DeleteAsync($"EmotionCheckIns/{id}");

    return await new ApiParsingUtils<EmotionCheckInDTO>().Process(response);
  }
}