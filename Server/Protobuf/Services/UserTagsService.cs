using DTO;
using Grpc.Net.Client;
using Protobuf.Services.Interfaces;
using Protobuf.Users;
using TagType = DTO.TagType;
using ProtoTagType = Protobuf.Users.TagType;

namespace Protobuf.Services;

public class UserTagsService : IUserTagsService
{
  public List<TagDto> GetAllTags(int userId)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Users.UserTagsService.UserTagsServiceClient(channel);

    var reply = client.GetAllTags(new UserId()
    {
      Id = userId
    });

    return reply.Tags.Select(tag => new TagDto
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