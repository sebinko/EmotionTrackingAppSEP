using Frontend.Data;
using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IUsersService
{
  public Task<User> Create(User user);
  public Task<AuthResponse?> GetByUsernameAndPassword(string username, string password);
}