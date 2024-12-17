using DTO;

namespace Frontend.Services.Interfaces;

public interface IUsersService
{
  Task<UserReturnDto?> GetById(int id);
}