using DTO;

namespace Frontend.Services.Interfaces;

public interface IUserTagsService
{
  public Task<List<TagDto>> GetAll();
}