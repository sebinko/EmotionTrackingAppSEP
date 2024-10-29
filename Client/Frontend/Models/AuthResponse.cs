using Frontend.Data;

namespace Frontend.Models;

public class AuthResponse
{
  public User user { get; set; }
  public string token { get; set; }
}