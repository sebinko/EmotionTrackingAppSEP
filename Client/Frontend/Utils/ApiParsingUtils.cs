using System.Net.Http;
using System.Text.Json;

namespace Frontend.Utils;

public class ApiParsingUtils<T>
{
  public async Task<T?> Process(HttpResponseMessage response)
  {
    if (!response.IsSuccessStatusCode)
    {
      throw new Exception($"InternalServerError: {response.ReasonPhrase}");
    }

    var content = await response.Content.ReadAsStringAsync();
    if (string.IsNullOrEmpty(content))
    {
      return default;
    }

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var result = JsonSerializer.Deserialize<T>(content, options);
    if (result == null)
    {
      throw new Exception("InternalServerError: Sequence contains no elements");
    }

    return result;
  }
}