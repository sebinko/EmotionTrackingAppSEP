using DTO;
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


    // print the user ids
    Console.WriteLine($"User1Id: {user1Id}, User2Id: {user2Id}");


    var existingFriendship = await client.GetFriendshipAsync(new FriendshipSimpleMessage
    {
      User1Id = user1Id,
      User2Id = user2Id
    });

    bool doesRequestExist = existingFriendship.User1Id != 0 || existingFriendship.User2Id != 0;
    bool isAccepted = existingFriendship.IsAccepted;
    int requestUserId = existingFriendship.User1Id == user1Id ? user1Id : user2Id;

    // print existing friendship
    Console.WriteLine($"Existing friendship: {existingFriendship}");

    Console.WriteLine($"Does request exist: {doesRequestExist}");

    if (!doesRequestExist)
    {
      await client.CreateFriendshipAsync(new FriendshipSimpleMessage
      {
        User1Id = user1Id,
        User2Id = user2Id
      });
      return;
    }

    if (doesRequestExist && !isAccepted && user1Id == requestUserId)
    {
      throw new RpcException(new Grpc.Core.Status(StatusCode.AlreadyExists,
        "You have already sent a friend request to this user"));
    }

    if (!isAccepted)
    {
      await client.UpdateFriendShipAsync(new FriendshipMessage
      {
        User1Id = user1Id,
        User2Id = user2Id,
        IsAccepted = true
      });
    }
    else
    {
      throw new RpcException(new Grpc.Core.Status(StatusCode.AlreadyExists,
        "User friendship already exists"));
    }
  }

  public async Task RemoveFriendship(int user1Id, int user2Id)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserFriendsService.UserFriendsServiceClient(channel);

    var reply = await client.RemoveFriendshipAsync(new FriendshipSimpleMessage
    {
      User1Id = user1Id,
      User2Id = user2Id
    });
  }

  public async Task<List<UserWithLatestCheckIn>> GetFriends(int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserFriendsService.UserFriendsServiceClient(channel);

    var reply = await client.GetAllFriendsAsync(new UserId()
    {
      Id = userId
    });

    return reply.Friends.Select(f => new UserWithLatestCheckIn()
    {
      User = new UserReturnDto()
      {
        Id = f.Friend.Id,
        Username = f.Friend.Username,
        Email = f.Friend.Email,
        CreatedAt = DateTime.Parse(f.Friend.CreatedAt),
        UpdatedAt = DateTime.Parse(f.Friend.UpdatedAt),
        Streak = f.Friend.Streak
      },
      LatestCheckIn = f.Emotion
    }).ToList();
  }
}