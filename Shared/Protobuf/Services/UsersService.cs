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

      var reply = await client.CreateAsync(new Users.User
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
      user.Streak = string.IsNullOrEmpty(reply.Streak) ? 0 : Convert.ToInt32(reply.Streak);
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
        Streak = string.IsNullOrEmpty(reply.Streak) ? 0 : Convert.ToInt32(reply.Streak),
        CreatedAt = createdAt,
        UpdatedAt = updatedAt
      };
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error getting user by username and password: {e.Message}");
    }
  }
}