using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IUserFriendsService
  {
    Task CreateFriendship(int user1Id, int user2Id);
    Task RemoveFriendship(int user1Id, int user2Id);
    Task<List<UserWithLatestCheckIn>> GetFriends(int userId);
  }
}