package dk.via.JavaDAO.Util;

import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.KeySpec;
import java.util.Base64;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;

/**
 * PasswordHasherUtil class to hash and verify passwords.
 */
public final class PasswordHasherUtil {

  /**
   * Number of iterations for PBKDF2.
   */
  private static final int ITERATIONS = 65536;
  /**
   * Length of the hash.
   */
  private static final int KEY_LENGTH = 256;
  static AppConfig appConfig;
  /**
   * Singleton instance of PasswordHasherUtil.
   */
  private static PasswordHasherUtil instance;
  /**
   * Secret key for hashing.
   */
  private static String SECRET_KEY;

  /**
   * Private constructor to prevent instantiation.
   */
  private PasswordHasherUtil() {
    SECRET_KEY = appConfig.getSecretKey();
  }

  public static void setAppConfig(AppConfig config) {
    appConfig = config;
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
   * Hashes the password using PBKDF2 and a secret key.
   *
   * @param password password to hash
   * @return hashed password
   */
  public String hashPassword(final String password) {
    try {
      KeySpec spec = new PBEKeySpec((SECRET_KEY + password).toCharArray(), SECRET_KEY.getBytes(),
          ITERATIONS, KEY_LENGTH);
      SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA256");
      byte[] hash = factory.generateSecret(spec).getEncoded();
      return Base64.getEncoder().encodeToString(hash);
    } catch (NoSuchAlgorithmException | InvalidKeySpecException e) {
      throw new RuntimeException("Error initializing PBKDF2 algorithm", e);
    }
  }

  /**
   * Verifies the password using PBKDF2 and a secret key.
   *
   * @param password password to verify
   * @param hash hashed password
   * @return true if password matches the hash, false otherwise
   */
  public boolean verifyPassword(final String password, final String hash) {
    String hashedPassword = hashPassword(password);
    return hashedPassword.equals(hash);
  }
}