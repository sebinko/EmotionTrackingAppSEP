using API.DTO;
using Grpc.Net.Client;
using Protobuf.Users;
using TagType = API.DTO.TagType;
using ProtoTagType = Protobuf.Users.TagType;

namespace Protobuf.Services;

public class UserTagsService
{
  public List<TagDTO> GetAllTags(UserReturnDTO userReturnDto)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserTagsService.UserTagsServiceClient(channel);

    var reply = client.GetAllTags(new UserId()
    {
      Id = userReturnDto.Id.Value
    });

    return reply.Tags.Select(tag => new TagDTO
    {
      Key = tag.Key,
      Type = ConvertTagType(tag.Type)
    }).ToList();
  }

  private TagType ConvertTagType(ProtoTagType protoTagType)
  {
    return protoTagType switch
    {
      ProtoTagType.What => TagType.WHAT,
      ProtoTagType.With => TagType.WITH,
      ProtoTagType.Where => TagType.WHERE,
      _ => throw new ArgumentOutOfRangeException(nameof(protoTagType), protoTagType, null)
    };
  }
}