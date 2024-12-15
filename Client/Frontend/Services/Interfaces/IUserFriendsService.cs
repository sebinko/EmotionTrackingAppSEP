using DTO;

namespace Frontend.Services.Interfaces;

public interface IUserFriendsService
{
  Task CreateFriendship(string user2Username);
  Task RemoveFriendship(string user2Username);
  Task<List<UserWithLatestCheckIn>> GetFriends();
}