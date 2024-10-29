using Entities;
using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IAuthService
{
  public Task<AuthResponse?> Register(User user);
  public Task<AuthResponse?> Login(string username, string password);
}