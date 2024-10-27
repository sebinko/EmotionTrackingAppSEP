using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.Auth;

public class AuthUtilities
{
  private IConfigurationRoot _configuration;

  public AuthUtilities()
  {
    _configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json")
      .Build();
  }

  public string GenerateJWTToken(User user)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Name, user.Username),
    };

    var jwtToken = new JwtSecurityToken(
      claims: claims,
      notBefore: DateTime.UtcNow,
      expires: DateTime.UtcNow.AddDays(30),
      signingCredentials: new SigningCredentials(
        new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JWT_Secret"])
        ),
        SecurityAlgorithms.HmacSha256Signature)
    );
    return new JwtSecurityTokenHandler().WriteToken(jwtToken);
  }
}