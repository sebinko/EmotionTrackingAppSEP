package dk.via.JavaDAO.Util;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class PasswordHasherUtilTest {
    private PasswordHasherUtil passwordHasherUtil;

    @BeforeEach
    public void setUp() {
        passwordHasherUtil = new PasswordHasherUtil().getInstance();
    }

    @Test
    public void testHashPassword() {
        String password = "mySecretPassword";
        String hash = passwordHasherUtil.hashPassword(password);
        assertNotNull(hash);
        assertNotEquals(password, hash);
    }

    @Test
    public void testVerifyPassword() {
        String password = "mySecretPassword";
        String hash = passwordHasherUtil.hashPassword(password);
        assertTrue(passwordHasherUtil.verifyPassword(password, hash));
    }

    @Test
    public void testVerifyPasswordWithWrongPassword() {
        String password = "mySecretPassword";
        String wrongPassword = "wrongPassword";
        String hash = passwordHasherUtil.hashPassword(password);
        assertFalse(passwordHasherUtil.verifyPassword(wrongPassword, hash));
    }
}