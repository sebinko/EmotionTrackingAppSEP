using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Utils;

public class AuthedClient(
  HttpClient client,
  AuthenticationStateProvider authenticationStateProvider)
{
  public async Task<HttpResponseMessage> GetAsync(string uri)
  {
    var user = await authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;


    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await client.GetAsync(uri);
  }
  
  public async Task<HttpResponseMessage> PatchAsync(string uri, HttpContent content)
  {
    var user = await authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await client.PatchAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
  {
    var user = await authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await client.PostAsync(uri, content);
  }

  public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent content)
  {
    var user = await authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await client.PutAsync(uri, content);
  }

  public async Task<HttpResponseMessage> DeleteAsync(string uri)
  {
    var user = await authenticationStateProvider.GetAuthenticationStateAsync();

    if (user.User.Identity is null || !user.User.Identity.IsAuthenticated)
      throw new Exception("User is not authenticated");

    var token = user.User.Claims.First(c => c.Type == "Token").Value;

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    return await client.DeleteAsync(uri);
  }
}