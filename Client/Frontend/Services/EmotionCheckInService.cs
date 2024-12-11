using System.Text;
using System.Text.Json;
using API.DTO;
using Frontend.Services.Interfaces;
using Frontend.Utils;
using SharedUtil;

namespace Frontend.Services;

public class EmotionCheckInService(AuthedClient httpClient) : IEmotionCheckInService
{
  
  public async Task<List<EmotionCheckInDTO>> GetAll()
  {
    var response = await httpClient.GetAsync("EmotionCheckIns");

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
      var response = await httpClient.GetAsync($"EmotionCheckIns/{id}");
  
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

  public async Task<EmotionCheckInDTO> Create(EmotionCheckInCreateDTO emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync("EmotionCheckIns", content);


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

  public async Task<EmotionCheckInDTO> Update(int id, EmotionCheckInUpdateDTO emotionCheckIn)
  {
    var json = JsonSerializer.Serialize(emotionCheckIn);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PatchAsync($"EmotionCheckIns", content);

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


  public async Task<EmotionCheckInDTO> Delete(int id)
  {
    var response = await httpClient.DeleteAsync($"EmotionCheckIns/{id}");

    if (response.IsSuccessStatusCode)
    {
      var deletedEmotionCheckIn = await response.Content.ReadFromJsonAsync<EmotionCheckInDTO>();
      return deletedEmotionCheckIn;
    }
    else
    {
      throw new Exception($"Failed to delete emotion check-in with ID: {id}");
    }
  }
}