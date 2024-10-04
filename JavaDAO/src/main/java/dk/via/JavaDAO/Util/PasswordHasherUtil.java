package dk.via.JavaDAO.Util;

import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

public class PasswordHasherUtil {
    private PasswordHasherUtil instance;
    private PasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

    public PasswordHasherUtil getInstance() {
        if (instance == null) {
            instance = new PasswordHasherUtil();
        }
        return instance;
    }

    public String hashPassword(String password) {
        return passwordEncoder.encode(password);
    }

    public boolean verifyPassword(String password, String hash) {
        return passwordEncoder.matches(password, hash);
    }
}