using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.Users;
using User = Entities.User;

namespace Protobuf.Services;

public class UserFriendsService
{
  public async Task CreateFriendship(int user1Id, int user2Id)
  {
    try
    {
      using var channel = GrpcChannel.ForAddress("http://localhost:8888");
      var client = new Users.UserFriendsService.UserFriendsServiceClient(channel);

      var reply = await client.CreateFriendshipAsync(new FriendshipMessage
      {
        User1Id = user1Id,
        User2Id = user2Id
      });
    }
    catch (RpcException e)
    {
      throw new Exception($"JavaDAO: Error creating user: {e.Message}");
    }
  }
}