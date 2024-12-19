using Protobuf.Services;

namespace API_Integration_Tests;

public class UserTagsServiceIntegrationTests
{
  private readonly UserTagsService service;

    public UserTagsServiceIntegrationTests()
    {
        service = new UserTagsService();
    }

    [Fact]
    public void GetAllTags_ShouldReturnTags()
    {
        int userId = 19;

        var result = service.GetAllTags(userId);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllTags_ShouldReturnEmptyList_WhenUserIdIsInvalid()
    {
        int invalidUserId = 9999;

        var result = service.GetAllTags(invalidUserId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}