using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Utils;

public class AuthedClient
{
  private readonly HttpClient _client;
  private readonly AuthenticationStateProvider _authenticationStateProvider;

  public AuthedClient(HttpClient client, AuthenticationStateProvider authenticationStateProvider, string baseUrl)
  {
    _client = client;
    _client.BaseAddress = new Uri(baseUrl);
    _authenticationStateProvider = authenticationStateProvider;
  }

  public async Task<HttpResponseMessage> GetAsync(string uri)
  {
    var user = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await _client.GetAsync(uri);
  }

  public async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent content)
  {
    var user = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await _client.PatchAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
  {
    var user = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await _client.PostAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent content)
  {
    var user = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await _client.PutAsync(uri, content);
  }

  public async Task<HttpResponseMessage> DeleteAsync(string uri)
  {
    var user = await _authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await _client.DeleteAsync(uri);
  }
  
  public string GetBaseAddress()
  {
    return _client.BaseAddress.ToString();
  }
}