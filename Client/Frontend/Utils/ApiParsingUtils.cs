using System.Text.Json;
using SharedUtil;

namespace Frontend.Utils;

public class ApiParsingUtils<T>
{
  public async Task<T?> Process(HttpResponseMessage response)
  {
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

    return JsonSerializer.Deserialize<T>(responseData, options);
  }
}