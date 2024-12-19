package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.AppConfig;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Util.PostgresConnector;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class UsersDAODBIntegrationTests {
  private UsersDAODB usersDAODB;
  private DBConnector dbConnector;
  private Connection connection;

  @BeforeEach
  void setUp() throws SQLException {
    dbConnector = new PostgresConnector(new AppConfig());
    connection = dbConnector.getConnection();
    usersDAODB = new UsersDAODB(dbConnector);
  }

  private void deleteUserIfExists(String username) throws SQLException {
    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".users WHERE username = ?";
    try (PreparedStatement statement = connection.prepareStatement(sql)) {
      statement.setString(1, username);
      statement.executeUpdate();
    }
  }

  @Test
  void testCreateUser() throws SQLException {
    deleteUserIfExists("testUser1");

    User user = new User("testUser1", "password", "user1@example.com");
    usersDAODB.Create(user);

    User fetchedUser = usersDAODB.GetSingle("testUser1", "password");
    assertNotNull(fetchedUser);
    assertEquals("testUser1", fetchedUser.getUsername());
  }

  @Test
  void testUpdateUser() throws SQLException {
    deleteUserIfExists("testUser2");

    User user = new User("testUser2", "password", "user2@example.com");
    usersDAODB.Create(user);

    user.setEmail("newemail@example.com");
    usersDAODB.Update(user);

    User updatedUser = usersDAODB.GetSingle("testUser2", "password");
    assertNotNull(updatedUser);
    assertEquals("newemail@example.com", updatedUser.getEmail());
  }


  @Test
  void testGetAllUsers() throws SQLException {
    deleteUserIfExists("testUser4");
    deleteUserIfExists("testUser5");

    User user1 = new User("testUser4", "password", "user4@example.com");
    User user2 = new User("testUser5", "password", "user5@example.com");
    usersDAODB.Create(user1);
    usersDAODB.Create(user2);

    List<User> users = usersDAODB.GetAll();
    assertNotNull(users);
    assertTrue(users.size() >= 2);
  }

  @Test
  void testCreateUserWithDuplicateUsername() throws SQLException {
    deleteUserIfExists("testUser6");

    User user = new User("testUser6", "password", "user6@example.com");
    usersDAODB.Create(user);

    User duplicateUser = new User("testUser6", "password", "duplicate@example.com");
    assertThrows(SQLException.class, () -> {
      usersDAODB.Create(duplicateUser);
    });
  }
}
