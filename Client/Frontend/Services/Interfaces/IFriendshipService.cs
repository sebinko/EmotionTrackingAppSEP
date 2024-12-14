using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IFriendshipService
{
  Task<List<FriendshipDTO>?> GetAll();
}