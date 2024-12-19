using DTO;
using Grpc.Core;
using Protobuf.Services;

namespace API_Integration_Tests;

public class UsersServiceIntegrationTests
{
  private readonly UsersService service;

  public UsersServiceIntegrationTests()
  {
    service = new UsersService();
  }

  [Fact]
  public async Task Create_ShouldCreateUser()
  {
    var user = new UserRegisterDto
    {
      Username = "newtestuser",
      Password = "password",
      Email = "newtestuser@gmail.com"
    };

    var result = await service.Create(user);

    Assert.NotNull(result);
    Assert.Equal(user.Username, result.Username);
    Assert.Equal(user.Email, result.Email);
  }
  [Fact]
  public async Task Create_ShouldThrowException_WhenUserIsInvalid()
  {
    var invalidUser = new UserRegisterDto
    {
      Username = "",
      Password = "password",
      Email = "invalidemail"
    };

    var exception = await Record.ExceptionAsync(() => service.Create(invalidUser));

    Assert.NotNull(exception);
    Assert.IsType<RpcException>(exception);
  }
  
  [Fact]
  public async Task GetByUsernameAndPassword_ShouldReturnUser()
  {
    var username = "newtestuser";
    var password = "password";

    var result = await service.GetByUsernameAndPassword(username, password);

    Assert.NotNull(result);
    Assert.Equal(username, result.Username);
  }
  
  [Fact]
  public async Task GetByUsernameAndPassword_ShouldReturnNull_WhenCredentialsAreInvalid()
  {
    var username = "invaliduser";
    var password = "wrongpassword";

    var result = await service.GetByUsernameAndPassword(username, password);

    Assert.Null(result);
  }

  [Fact]
  public async Task GetByUsername_ShouldReturnUser()
  {
    var username = "newtestuser";

    var result = await service.GetByUsername(username);

    Assert.NotNull(result);
    Assert.Equal(username, result.Username);
  }
  
  [Fact]
  public async Task ChangePassword_ShouldReturnNull_WhenUserIdIsInvalid()
  {
    int invalidUserId = 9999;
    var changePasswordDto = new ChangePasswordDto
    {
      NewPassword = "password"
    };

    var exception = await Record.ExceptionAsync(() => service.ChangePassword(invalidUserId, changePasswordDto));
    
    Assert.Null(exception);
  }

  [Fact]
  public async Task ChangePassword_ShouldChangePassword()
  {
    int userId = 22;
    var changePasswordDto = new ChangePasswordDto
    {
      NewPassword = "password"
    };

    var result = await service.ChangePassword(userId, changePasswordDto);

    Assert.NotNull(result);
    Assert.Equal(userId, result.Id);
  }

  [Fact]
  public async Task GetById_ShouldReturnUser()
  {
    int userId = 22;

    var result = await service.GetById(userId);

    Assert.NotNull(result);
    Assert.Equal(userId, result.Id);
  }
  
  [Fact]
  public async Task GetById_ShouldReturnNull_WhenUserIdIsInvalid()
  {
    int invalidUserId = 9999;

    var result = await service.GetById(invalidUserId);

    Assert.Null(result);
  }
}