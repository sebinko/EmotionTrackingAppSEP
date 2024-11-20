using System.Text.Json;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Frontend.Services.Interfaces;

namespace Frontend.Services
{
  public class LocalStorageService : IStorageService
  {
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
      _jsRuntime = jsRuntime;
    }

    public async Task<T> GetItem<T>(string key)
    {
      var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

      if (typeof(T) == typeof(string))
      {
        return (T)(object)json;
      }
      
      return JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetItem<T>(string key, T value)
    {
      var json = JsonSerializer.Serialize(value);
      await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task RemoveItem(string key)
    {
      await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
  }
}