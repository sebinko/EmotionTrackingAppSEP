using API.Utilities;
using NUnit.Framework;

namespace API_Test.Utilities;

[TestFixture]
public class PasswordHasherUtilTests
{
  private const string SecretKey = "VIAUniversityCollege";

  [Test]
  public void HashPassword_ShouldReturnHashedPassword()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);
    var password = "myPassword";

    var hash = passwordHasher.HashPassword(password);

    Assert.IsFalse(string.IsNullOrEmpty(hash));
  }

  [Test]
  public void VerifyPassword_ShouldReturnTrueForValidPassword()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);
    var password = "myPassword";
    var hash = passwordHasher.HashPassword(password);

    var result = passwordHasher.VerifyPassword(password, hash);

    Assert.IsTrue(result);
  }

  [Test]
  public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);
    var password = "myPassword";
    var wrongPassword = "wrongPassword";
    var hash = passwordHasher.HashPassword(password);

    var result = passwordHasher.VerifyPassword(wrongPassword, hash);

    Assert.IsFalse(result);
  }

  [Test]
  public void HashPassword_ShouldThrowArgumentExceptionForNullPassword()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);

    Assert.Throws<ArgumentException>(() => passwordHasher.HashPassword(null));
  }

  [Test]
  public void VerifyPassword_ShouldThrowArgumentExceptionForNullHash()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);
    var password = "myPassword";

    Assert.Throws<ArgumentException>(() => passwordHasher.VerifyPassword(password, null));
  }

  [Test]
  public void HashPassword_ShouldReturnExpectedHashForPassword()
  {
    var passwordHasher = new PasswordHasherUtil(SecretKey);
    var password = "password";
    var expectedHash = "l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=";

    var hash = passwordHasher.HashPassword(password);

    Assert.That(hash, Is.EqualTo(expectedHash));
  }
}