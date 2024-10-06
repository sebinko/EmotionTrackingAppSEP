package dk.via.JavaDAO.Util;

import java.util.Properties;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public final class AppConfig {

  private final Properties properties = new Properties();
  private final Logger logger = LoggerFactory.getLogger(AppConfig.class.getName());

  public AppConfig() {
    try {
      properties.load(getClass().getClassLoader().getResourceAsStream("app.properties"));
    } catch (Exception e) {
      logger.error("Error loading properties file: {}", e.getMessage());
    }
  }

  public String getDbUrl() {
    return properties.getProperty("db.url") == null ? "jdbc:postgresql://localhost:5432/"
        : properties.getProperty("db.url");
  }

  public String getDbUser() {
    return properties.getProperty("db.user") == null ? "postgres"
        : properties.getProperty("db.user");
  }

  public String getDbPassword() {
    return properties.getProperty("db.password") == null ? ""
        : properties.getProperty("db.password");
  }
}
