using System.Net.Http;
using System.Text.Json;
using SharedUtil;

namespace Frontend.Utils
{
  public partial class ApiParsingUtils<T>
  {
    public async Task<T?> Process(HttpResponseMessage response)
    {
      var content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        try
        {
          var errorResponse = JsonSerializer.Deserialize<ApiExceptionResponse>(content, new JsonSerializerOptions
          {
            PropertyNameCaseInsensitive = true
          });

          if (errorResponse != null)
          {
            throw new Exception($"Error: {errorResponse.Error}, Type: {errorResponse.Type}");
          }
        }
        catch (JsonException)
        {
          throw new Exception($"InternalServerError: {response.ReasonPhrase}");
        }
      }

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

  public partial class ApiParsingUtils
  {
    public async Task<object?> Process(HttpResponseMessage response)
    {
      var content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        try
        {
          var errorResponse = JsonSerializer.Deserialize<ApiExceptionResponse>(content, new JsonSerializerOptions
          {
            PropertyNameCaseInsensitive = true
          });

          if (errorResponse != null)
          {
            throw new Exception($"Error: {errorResponse.Error}, Type: {errorResponse.Type}");
          }
        }
        catch (JsonException)
        {
          throw new Exception($"InternalServerError: {response.ReasonPhrase}");
        }
      }

      if (string.IsNullOrEmpty(content))
      {
        return null;
      }

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      var result = JsonSerializer.Deserialize<object>(content, options);
      if (result == null)
      {
        throw new Exception("InternalServerError: Sequence contains no elements");
      }

      return result;
    }
  }
}