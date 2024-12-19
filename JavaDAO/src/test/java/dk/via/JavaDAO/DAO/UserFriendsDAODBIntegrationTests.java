package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Friendship;
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

public class UserFriendsDAODBIntegrationTests {
  private UserFriendsDAODB userFriendsDAODB;
  private DBConnector dbConnector;
  private Connection connection;
  private UsersDAO usersDAO;

  @BeforeEach
  void setUp() throws SQLException {
    dbConnector = new PostgresConnector(new AppConfig());
    connection = dbConnector.getConnection();
    usersDAO = new UsersDAODB(dbConnector);
    userFriendsDAODB = new UserFriendsDAODB(dbConnector, usersDAO);
  }

  private void deleteUserIfExists(String username) throws SQLException {
    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".users WHERE username = ?";
    try (PreparedStatement statement = connection.prepareStatement(sql)) {
      statement.setString(1, username);
      statement.executeUpdate();
    }
  }

  @Test
  void testAddFriend() throws SQLException {
    deleteUserIfExists("testUser1");
    deleteUserIfExists("testUser2");

    User user = new User("testUser1", "password", "user11@example.com");
    User friend = new User("testUser2", "password", "friend11@example.com");
    usersDAO.Create(user);
    usersDAO.Create(friend);

    userFriendsDAODB.CreateFriendship(user.getId(), friend.getId());

    Friendship friendship = userFriendsDAODB.GetFriendShip(user.getId(), friend.getId());
    assertNotNull(friendship);
    assertEquals(user.getId(), friendship.getUser1Id());
    assertEquals(friend.getId(), friendship.getUser2Id());
  }

  @Test
  void testRemoveFriend() throws SQLException {
    deleteUserIfExists("testUser1");
    deleteUserIfExists("testUser2");

    User user = new User("testUser1", "password", "user3@example.com");
    User friend = new User("testUser2", "password", "friend3@example.com");
    usersDAO.Create(user);
    usersDAO.Create(friend);

    userFriendsDAODB.CreateFriendship(user.getId(), friend.getId());
    userFriendsDAODB.RemoveFriendship(user.getId(), friend.getId());

    Friendship friendship = userFriendsDAODB.GetFriendShip(user.getId(), friend.getId());
    assertNull(friendship);
  }

  @Test
  void testUpdateFriendship() throws SQLException {
    deleteUserIfExists("testUser1");
    deleteUserIfExists("testUser2");

    User user = new User("testUser1", "password", "user5@example.com");
    User friend = new User("testUser2", "password", "friend5@example.com");
    usersDAO.Create(user);
    usersDAO.Create(friend);

    userFriendsDAODB.CreateFriendship(user.getId(), friend.getId());

    Friendship friendship = userFriendsDAODB.GetFriendShip(user.getId(), friend.getId());
    assertNotNull(friendship);
    assertEquals(user.getId(), friendship.getUser1Id());
    assertEquals(friend.getId(), friendship.getUser2Id());

    friendship.setAccepted(true);
    userFriendsDAODB.UpdateFriendship(friendship);

    Friendship updatedFriendship = userFriendsDAODB.GetFriendShip(user.getId(), friend.getId());
    assertNotNull(updatedFriendship);
    assertEquals(true, updatedFriendship.isAccepted());
  }

  @Test
  void testCreateFriendshipWithNonExistentUser() throws SQLException {
    deleteUserIfExists("testUser1");

    User user = new User("testUser1", "password", "user1@example.com");
    usersDAO.Create(user);

    assertThrows(SQLException.class, () -> {
      userFriendsDAODB.CreateFriendship(user.getId(), 9999); // Non-existent user ID
    });
  }
}
