using API.DTO;
using Entities;

namespace Frontend.Services.Interfaces;

public interface IAuthService
{
  public Task<UserWithTokenDTO?> Register(User user);
  public Task<UserWithTokenDTO?> Login(string username, string password);
  public Task <UserWithTokenDTO?> GetUser();
  public Task Logout();
}