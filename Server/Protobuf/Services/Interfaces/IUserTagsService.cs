using DTO;

namespace Protobuf.Services.Interfaces
{
  public interface IUserTagsService
  {
    List<TagDto> GetAllTags(int userId);
  }
}