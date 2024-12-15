
using System;
using System.Security.Cryptography;
using System.Text;

namespace API.Utilities;

public class PasswordHasherUtil(string secretKey)
{
  private const int Iterations = 65536;

  private const int KeyLength = 32; // 256 bits

  public string HashPassword(string password)
  {
    if (string.IsNullOrEmpty(password))
      throw new ArgumentException("Password cannot be null or empty.", nameof(password));

    using var hmac = new Rfc2898DeriveBytes(secretKey + password,
      Encoding.UTF8.GetBytes(secretKey), Iterations, HashAlgorithmName.SHA256);
    var hash = hmac.GetBytes(KeyLength);
    return Convert.ToBase64String(hash);
  }

  public bool VerifyPassword(string password, string hash)
  {
    if (string.IsNullOrEmpty(hash))
      throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

    var hashedPassword = HashPassword(password);
    return hashedPassword == hash;
  }
}