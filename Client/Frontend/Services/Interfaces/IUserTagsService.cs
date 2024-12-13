using API.DTO;

namespace Frontend.Services.Interfaces;

public interface IUserTagsService
{
  public Task<List<TagDTO>> GetAll();
}