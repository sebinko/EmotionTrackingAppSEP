package dk.via.JavaDAO.Util;

import com.google.inject.Inject;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


/**
 * Class for connecting to the database
 */
public final class PostgresConnector implements DBConnector {

  /**
   * The logger for the DBConnector class
   */
  private static final Logger logger = LoggerFactory.getLogger(PostgresConnector.class.getName());
  /**
   * The connection to the database
   */
  private Connection connection;

  /**
   * Constructor for the DBConnector class
   */
  @Inject
  public PostgresConnector(AppConfig appConfig) {
    try {
      logger.info(appConfig.getDbPassword());
      connection = DriverManager.getConnection(appConfig.getDbUrl(), appConfig.getDbUser(),
          appConfig.getDbPassword());
      logger.info("Connected to database {}", appConfig.getDbUrl());
    } catch (SQLException e) {
      logger.error("Error connecting to database: {}", e.getMessage());
    }

  }

  /**
   * Execute an update query on the database
   *
   * @param sql the query to execute
   * @param params the parameters to the query
   */
  public void executeUpdate(String sql, Object... params) {
    try (PreparedStatement preparedStatement = connection.prepareStatement(sql)) {
      for (int i = 0; i < params.length; i++) {
        preparedStatement.setObject(i + 1, params[i]);
      }
      preparedStatement.executeUpdate();
    } catch (SQLException e) {
      logger.error("Error executing update: {}", e.getMessage());
    }
  }

  /**
   * Get the connection to the database
   *
   * @return the connection
   */
  public Connection getConnection() {
    return connection;
  }
}
