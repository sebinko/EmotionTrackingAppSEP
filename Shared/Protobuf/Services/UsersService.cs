using API.DTO;
using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.Users;
using User = Entities.User;

namespace Protobuf.Services;

public class UsersService
{
  public async Task<User> Create(User user)
  {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new Users.UsersService.UsersServiceClient(channel);

      var reply = await client.CreateAsync(new Users.UserCreate
      {
        Username = user.Username,
        Password = user.Password,
        Email = user.Email
      });

      DateTime.TryParse(reply.CreatedAt, out var createdAt);
      DateTime.TryParse(reply.UpdatedAt, out var updatedAt);

      user.Id = Convert.ToInt32(reply.Id);
      user.Username = reply.Username;
      user.Email = reply.Email;
      user.Streak = reply.Streak;
      user.CreatedAt = createdAt;
      user.UpdatedAt = updatedAt;
      return user;
  }

  public async Task<User> GetByUsernameAndPassword(string username, string password)
  {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new Users.UsersService.UsersServiceClient(channel);

      var reply = await client.GetByUsernameAndPasswordAsync(
        new UsernameAndPassword
        {
          Username = username,
          Password = password
        });

      DateTime.TryParse(reply.CreatedAt, out var createdAt);
      DateTime.TryParse(reply.UpdatedAt, out var updatedAt);

      return new User
      {
        Id = Convert.ToInt32(reply.Id),
        Username = reply.Username,
        Email = reply.Email,
        Streak = reply.Streak,
        CreatedAt = createdAt,
        UpdatedAt = updatedAt
      };
  }

  public async Task<User> ChangePassword(int userId, ChangePasswordDTO changePasswordDto)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UsersService.UsersServiceClient(channel);

    var user = await client.GetByIdAsync(new UserId
    {
      Id = userId
    });

    if (user == null)
    {
      throw new Exception("User not found");
    }

    var reply = await client.UpdateAsync(new UserUpdate
    {
      Id = user.Id,
      Username = user.Username,
      Password = changePasswordDto.NewPassword,
      Email = user.Email
    });

    DateTime.TryParse(reply.CreatedAt, out var createdAt);
    DateTime.TryParse(reply.UpdatedAt, out var updatedAt);

    return new User
    {
      Id = Convert.ToInt32(reply.Id),
      Username = reply.Username,
      Password = null,
      Email = reply.Email,
      Streak = reply.Streak,
      CreatedAt = createdAt,
      UpdatedAt = updatedAt
    };
  }
}