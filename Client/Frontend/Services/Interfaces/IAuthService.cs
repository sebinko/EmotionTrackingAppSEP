using API.DTO;
using Entities;
using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IAuthService
{
  public Task<UserWithTokenDTO?> Register(User user);
  public Task<UserWithTokenDTO?> Login(string username, string password);
  public Task <UserWithTokenDTO?> GetUser();
  public Task Logout();
}