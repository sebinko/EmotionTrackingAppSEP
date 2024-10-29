using Frontend.Services.Interfaces;

namespace Frontend.Services;

public class StatusService : IStatusService
{
  private readonly HttpClient _httpClient;

  public StatusService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<(bool isOkay, string message, string statusCode)> GetStatusAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync("http://localhost:5195/Status");

      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        return (true, content, "OK");
      }
      else
      {
        var content = await response.Content.ReadAsStringAsync();
        return (false, content, "NOT OK");
      }
    }
    catch (Exception e)
    {
      return (false, e.Message, "ERROR");
    }
  }
}