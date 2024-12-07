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
    try
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
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating user: {e.Message}");
    }
  }

  public async Task<User> GetByUsernameAndPassword(string username, string password)
  {
    try
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
        Password = reply.Password,
        Email = reply.Email,
        Streak =reply.Streak,
        CreatedAt = createdAt,
        UpdatedAt = updatedAt
      };
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error getting user by username and password: {e.Message}");
    }
  }

  public async Task<User> ChangePassword(int userId, ChangePasswordDTO changePasswordDto)
  {
    try
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

      user.Password = changePasswordDto.NewPassword;

      var reply = await client.UpdateAsync(user);

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
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error changing password: {e.Message}");
    }
  }
}