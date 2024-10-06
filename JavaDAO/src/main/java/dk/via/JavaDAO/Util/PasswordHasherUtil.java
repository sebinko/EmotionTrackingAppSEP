package dk.via.JavaDAO.Util;

import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

/**
 * PasswordHasherUtil class to hash and verify passwords.
 */
public final class PasswordHasherUtil {

  /**
   * Singleton instance of PasswordHasherUtil.
   */
  private static PasswordHasherUtil instance;
  /**
   * Password encoder to hash and verify passwords.
   */
  private final PasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

  /**
   * Private constructor to prevent instantiation.
   */
  private PasswordHasherUtil() {
  }

  /**
   * Singleton instance of PasswordHasherUtil.
   *
   * @return instance of PasswordHasherUtil
   */
  public static PasswordHasherUtil getInstance() {
    if (instance == null) {
      instance = new PasswordHasherUtil();
    }
    return instance;
  }

  /**
   * Hashes the password using BCryptPasswordEncoder.
   *
   * @param password password to hash
   * @return hashed password
   */
  public String hashPassword(final String password) {
    return passwordEncoder.encode(password);
  }

  /**
   * Verifies the password using BCryptPasswordEncoder.
   *
   * @param password password to verify
   * @param hash hashed password
   * @return true if password matches the hash, false otherwise
   */
  public boolean verifyPassword(final String password, final String hash) {
    return passwordEncoder.matches(password, hash);
  }
}
