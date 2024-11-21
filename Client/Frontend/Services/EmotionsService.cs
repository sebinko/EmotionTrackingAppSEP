using System.Text.Json;
using API.DTO;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Frontend.Services;

public class EmotionsService(HttpClient httpClient) : IEmotionsService
{
  public async Task<List<EmotionDTO>?> GetAll(string? emotionQuery, string? emotionColor)
  {
    var response = await httpClient.GetAsync(_buildEmotionUrl(emotionQuery, emotionColor));
    response.EnsureSuccessStatusCode();
    var data = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<List<EmotionDTO>>(data, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    });
  }

  private static string _buildEmotionUrl(string? emotionQuery = null, string? emotionColor = null)
  {
    var path = "/Emotions";
    var queryParameters = new Dictionary<string, string>();

    // Add parameters conditionally
    if (!string.IsNullOrWhiteSpace(emotionQuery))
    {
      queryParameters["EmotionQuery"] = emotionQuery;
    }

    if (!string.IsNullOrWhiteSpace(emotionColor))
    {
      queryParameters["EmotionColor"] = emotionColor;
    }

    // Return the built URL
    return queryParameters.Count > 0
      ? QueryHelpers.AddQueryString(path, queryParameters)
      : path;
  }
}