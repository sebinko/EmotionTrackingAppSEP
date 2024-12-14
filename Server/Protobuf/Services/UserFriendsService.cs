using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.Services.Interfaces;
using Protobuf.Users;

namespace Protobuf.Services;

public class UserFriendsService : IUserFriendsService
{
  public async Task CreateFriendship(int user1Id, int user2Id)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserFriendsService.UserFriendsServiceClient(channel);

    var reply = await client.CreateFriendshipAsync(new FriendshipMessage
    {
      User1Id = user1Id,
      User2Id = user2Id
    });
  }
  
  public async Task RemoveFriendship(int user1Id, int user2Id)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserFriendsService.UserFriendsServiceClient(channel);

    var reply = await client.RemoveFriendshipAsync(new FriendshipMessage
    {
      User1Id = user1Id,
      User2Id = user2Id
    });
  }
}