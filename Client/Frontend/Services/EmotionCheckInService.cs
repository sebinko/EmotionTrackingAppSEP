using System.Text;
using System.Text.Json;
using API.DTO;
using SharedUtil;

namespace Frontend.Services.Interfaces;

public class EmotionCheckInService (HttpClient httpClient): IEmotionCheckInService
{
  public async Task<List<EmotionCheckInDTO>> GetAll(string token)
  {
    var request = new HttpRequestMessage(HttpMethod.Get, "EmotionCheckIns");
    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    
    var response = await httpClient.SendAsync(request);
    
    if (!response.IsSuccessStatusCode)
    {
      var exStr = await response.Content.ReadAsStringAsync();
      var apiException = JsonSerializer.Deserialize<ApiExceptionResponse>(exStr,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });
      
      if (apiException is not null)
        throw new Exception($"{response.StatusCode}: {apiException.Error}");
      throw new Exception($"{response.StatusCode}: {exStr}");
    }
    
    var responseData = await response.Content.ReadAsStringAsync();
    
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    
    return JsonSerializer.Deserialize<List<EmotionCheckInDTO>>(responseData, options);
  }

  public async Task<EmotionCheckInDTO> Get(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn, string token)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    
   
    var request = new HttpRequestMessage(HttpMethod.Post, "EmotionCheckIns")
    {
      Content = content
    };
    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    var response = await httpClient.SendAsync(request);

    
    if (!response.IsSuccessStatusCode)
    {
      var exStr = await response.Content.ReadAsStringAsync();
      var apiException = JsonSerializer.Deserialize<ApiExceptionResponse>(exStr,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });
      
      if (apiException is not null)
        throw new Exception($"{response.StatusCode}: {apiException.Error}");
      throw new Exception($"{response.StatusCode}: {exStr}");
    }
    
    var responseData = await response.Content.ReadAsStringAsync();
    
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };
    
    return JsonSerializer.Deserialize<EmotionCheckInDTO>(responseData, options);
  }

  public async Task<EmotionCheckInDTO> Update(int id, EmotionCheckInDTO emotionCheckIn)
  {
    throw new NotImplementedException();
  }

  public async Task<EmotionCheckInDTO> Delete(int id)
  {
    throw new NotImplementedException();
  }
}