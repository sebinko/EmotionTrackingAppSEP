using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IUsersService
  {
    Task<UserReturnDto> Create(UserRegisterDto user);
    Task<UserReturnDto?> GetByUsernameAndPassword(string username, string password);
    Task<UserReturnDto?> ChangePassword(int userId, ChangePasswordDto changePasswordDto);
    Task<UserReturnDto?> GetById(int id);
  }
}