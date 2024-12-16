using System.Net.Http.Headers;

namespace Frontend.Utils;

public class NonAuthedClient
{
  private readonly HttpClient _client;

  public NonAuthedClient(HttpClient client, string baseUrl)
  {
    _client = client;
    _client.BaseAddress = new Uri(baseUrl);
  }

  public async Task<HttpResponseMessage> GetAsync(string uri)
  {
    return await _client.GetAsync(uri);
  }

  public async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent content)
  {
    return await _client.PatchAsync(uri, content);
  }
  
  public async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent content, Dictionary<string, string> headers)
  {
    foreach (var header in headers)
    {
      _client.DefaultRequestHeaders.Add(header.Key, header.Value);
    }
    return await _client.PatchAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
  {
    return await _client.PostAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent content)
  {
    return await _client.PutAsync(uri, content);
  }

  public async Task<HttpResponseMessage> DeleteAsync(string uri)
  {
    return await _client.DeleteAsync(uri);
  }
  
  public string GetBaseAddress()
  {
    return _client.BaseAddress.ToString();
  }
}