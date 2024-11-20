using Blazored.SessionStorage;
using System.Text.Json;
using System.Threading.Tasks;
using Frontend.Services.Interfaces;

public class SessionStorageService : IStorageService
{
  private readonly ISessionStorageService _sessionStorage;

  public SessionStorageService(ISessionStorageService sessionStorage)
  {
    _sessionStorage = sessionStorage;
  }

  public async Task<T> GetItem<T>(string key)
  {
    return await _sessionStorage.GetItemAsync<T>(key);
  }

  public async Task SetItem<T>(string key, T value)
  {
    await _sessionStorage.SetItemAsync(key, value);
  }

  public async Task RemoveItem(string key)
  {
    await _sessionStorage.RemoveItemAsync(key);
  }
}