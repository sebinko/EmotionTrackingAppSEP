using Grpc.Core;
using Protobuf.Services;

namespace API_Integration_Tests;

public class UserFriendsServiceIntegrationTests
{
  private readonly UserFriendsService service;

  public UserFriendsServiceIntegrationTests()
  {
    service = new UserFriendsService();
  }

  [Fact]
  public async Task CreateFriendship_ShouldCreateFriendship()
  {
    int user1Id = 19;
    int user2Id = 2;

    var exception = await Record.ExceptionAsync(() => service.CreateFriendship(user1Id, user2Id));

    Assert.Null(exception);
  }

  [Fact]
  public async Task CreateFriendship_ShouldThrowException_WhenUserIdsAreInvalid()
  {
    int invalidUserId1 = -1;
    int invalidUserId2 = -2;

    var exception = await Record.ExceptionAsync(() => service.CreateFriendship(invalidUserId1, invalidUserId2));

    Assert.NotNull(exception);
    Assert.IsType<RpcException>(exception);
  }
  
  [Fact]
  public async Task RemoveFriendship_ShouldRemoveFriendship()
  {
    int user1Id = 19;
    int user2Id = 2;

    var exception = await Record.ExceptionAsync(() => service.RemoveFriendship(user1Id, user2Id));

    Assert.Null(exception);
  }

  [Fact]
  public async Task RemoveFriendship_ShouldThrowException_WhenUserIdsAreInvalid()
  {
    int invalidUserId1 = 9999;
    int invalidUserId2 = 9999;

    var exception = await Record.ExceptionAsync(() => service.RemoveFriendship(invalidUserId1, invalidUserId2));
    
    Assert.Null(exception);
  }
  
  [Fact]
  public async Task GetFriends_ShouldReturnFriendsList()
  {
    int userId = 19;

    var result = await service.GetFriends(userId);

    Assert.NotNull(result);
    Assert.NotEmpty(result);
  }
  
  [Fact]
  public async Task GetFriends_ShouldReturnEmptyList_WhenUserIdIsInvalid()
  {
    int invalidUserId = 9999;

    var result = await service.GetFriends(invalidUserId);

    Assert.NotNull(result);
    Assert.Empty(result);
  }
}