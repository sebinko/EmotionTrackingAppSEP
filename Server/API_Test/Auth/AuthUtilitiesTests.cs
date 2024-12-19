using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace API.Auth.Tests
{
  [TestFixture]
  public class AuthUtilitiesTests
  {
    private Mock<IConfigurationRoot> _configurationMock;
    private AuthUtilities _authUtilities;

    [SetUp]
    public void SetUp()
    {
      _configurationMock = new Mock<IConfigurationRoot>();
      _configurationMock.Setup(config => config["ApplicationSettings:JWT_Secret"]).Returns("your_secret_key_here");
      _authUtilities = new AuthUtilities();
    }

    [Test]
    public void GenerateJwtToken_ShouldReturnValidToken()
    {
      var user = new UserReturnDto { Id = 1, Username = "testuser", Email = "testuser@example.com" };
      
      var token = _authUtilities.GenerateJwtToken(user);
      
      Assert.IsNotNull(token);
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadJwtToken(token) as JwtSecurityToken;
      Assert.IsNotNull(jwtToken);
      Assert.AreEqual(user.Id.ToString(), jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
      Assert.AreEqual(user.Username, jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value);
    }
  }
}