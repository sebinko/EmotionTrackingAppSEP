using Entities;
using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class UsersService
{
  public async Task<User> Create(User user)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new Protobuf.Users.UsersService.UsersServiceClient(channel);

      var reply = await client.CreateAsync(new Protobuf.Users.User()
      {
        Username = user.Username,
        Password = user.Password,
        Email = user.Email
      });

      user.Id = Convert.ToInt32(reply.Id);
      user.Username = reply.Username;
      user.Email = reply.Email;
      user.CreatedAt = DateTime.Parse(reply.CreatedAt);
      user.UpdatedAt = DateTime.Parse(reply.UpdatedAt);
      return user;
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating user: {e.Message}");
    }
    catch (Exception e)
    {
      throw new Exception($"JavaDAO: {e.Message}");
    }
  }

  public async Task<User> GetByUsernameAndPassword(string username, string password)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new Protobuf.Users.UsersService.UsersServiceClient(channel);

      var reply = await client.GetByUsernameAndPasswordAsync(new Protobuf.Users.UsernameAndPassword()
      {
        Username = username,
        Password = password
      });

      DateTime createdAt = DateTime.Parse(reply.CreatedAt);
      DateTime updatedAt = DateTime.Parse(reply.UpdatedAt);
      return new User()
      {
        Id = Convert.ToInt32(reply.Id),
        Username = reply.Username,
        Password = reply.Password,
        Email = reply.Email,
        // Streak = Convert.ToInt32(reply.Streak),
        CreatedAt = createdAt,
        UpdatedAt = updatedAt
      };
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error getting user by username and password: {e.Message}");
    }
    catch (Exception e)
    {
      throw e;
    }
    
    
  }
}