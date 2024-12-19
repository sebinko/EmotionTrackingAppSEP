package dk.via.JavaDAO.DAO;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.*;
import java.util.ArrayList;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class UsersDAODBTest {

  private DBConnector mockConnector;
  private Connection mockConnection;
  private PreparedStatement mockStatement;
  private ResultSet mockResultSet;

  private UsersDAODB usersDAODB;

  @BeforeEach
  void setUp() throws SQLException {
    // Mocks for the necessary objects
    mockConnector = mock(DBConnector.class);
    mockConnection = mock(Connection.class);
    mockStatement = mock(PreparedStatement.class);
    mockResultSet = mock(ResultSet.class); // Initialize mockResultSet
    when(mockConnector.getConnection()).thenReturn(mockConnection);
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);

    usersDAODB = new UsersDAODB(mockConnector);
  }


  @Test
  void testGetAll() throws SQLException {
    String sql = "select * from \"EmotionsTrackingWebsite\".users_with_streaks;";
    when(mockConnection.prepareStatement(sql)).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);

    // Simulate two rows in the result set
    when(mockResultSet.next()).thenReturn(true, true, false);
    when(mockResultSet.getInt("id")).thenReturn(1, 2);
    when(mockResultSet.getString("username")).thenReturn("User1", "User2");
    when(mockResultSet.getString("password")).thenReturn("Password1", "Password2");
    when(mockResultSet.getString("email")).thenReturn("email1@test.com", "email2@test.com");
    when(mockResultSet.getTimestamp("created_at")).thenReturn(new Timestamp(0), new Timestamp(0));
    when(mockResultSet.getTimestamp("updated_at")).thenReturn(new Timestamp(0), new Timestamp(0));
    when(mockResultSet.getInt("current_streak")).thenReturn(5, 10);

    ArrayList<User> users = usersDAODB.GetAll();

    assertEquals(2, users.size());
    assertEquals("User1", users.get(0).getUsername());
    assertEquals(5, users.get(0).getStreak());
    assertEquals("User2", users.get(1).getUsername());
    assertEquals(10, users.get(1).getStreak());

    verify(mockConnection).prepareStatement(sql);
    verify(mockStatement).executeQuery();
  }

  @Test
  void testGetSingleById_Found() throws SQLException {
    String sql = "select * from \"EmotionsTrackingWebsite\".users_with_streaks where id = ?;";
    when(mockConnection.prepareStatement(sql)).thenReturn(mockStatement);

    // Simulating query execution and result
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("id")).thenReturn(1);
    when(mockResultSet.getString("username")).thenReturn("User1");
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);

    User user = usersDAODB.GetSingle(1);

    assertNotNull(user);
    assertEquals(1, user.getId());
    assertEquals("User1", user.getUsername());
    verify(mockStatement).setInt(1, 1);
    verify(mockStatement).executeQuery();
  }

  @Test
  void testCreate_ShouldReturnUserWithGeneratedId() throws SQLException {
    // Arrange
    User userToCreate = new User(0, "TestUser", "TestPassword", "test@test.com", null, null, 0);

    // Initialize mock ResultSet and check the sequence of method invocations
    ResultSet mockResultSet = mock(ResultSet.class);  // Ensure ResultSet is mocked correctly
    when(mockResultSet.next()).thenReturn(true, false);  // Simulate that there is one row in the result set
    when(mockResultSet.getInt("id")).thenReturn(1);  // Return a mock ID
    when(mockResultSet.getString("username")).thenReturn("TestUser");  // Mock username column
    when(mockResultSet.getString("password")).thenReturn("TestPassword");
    when(mockResultSet.getString("email")).thenReturn("test@test.com");
    when(mockResultSet.getTimestamp("created_at")).thenReturn(Timestamp.valueOf("2024-06-10 10:00:00"));
    when(mockResultSet.getTimestamp("updated_at")).thenReturn(Timestamp.valueOf("2024-06-10 10:00:00"));

    // Mocking prepareStatement and executeQuery
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet); // Ensure executeQuery returns mockResultSet

    // Act: Call the method under test
    User createdUser = usersDAODB.Create(userToCreate);

    // Assert
    assertNotNull(createdUser, "Created user should not be null.");
    assertEquals(1, createdUser.getId(), "User ID should be 1.");
    assertEquals("TestUser", createdUser.getUsername(), "Username mismatch.");
    assertEquals("test@test.com", createdUser.getEmail(), "Email mismatch.");

    // Strip milliseconds for accurate comparison
    String expectedTime = "2024-06-10 10:00:00";
    String actualTime = createdUser.getCreatedAt().toString().substring(0, 19); // Strips milliseconds
    assertEquals(expectedTime, actualTime, "Created at mismatch.");

    // Verify the interaction calls
    verify(mockStatement, times(1)).executeQuery();  // Ensure that executeQuery was called
    verify(mockStatement, times(1)).setString(1, "TestUser"); // Verify username setting
    verify(mockStatement, times(1)).setString(2, "TestPassword"); // Verify password setting
    verify(mockStatement, times(1)).setString(3, "test@test.com"); // Verify email setting
  }





  @Test
  void testUpdate() throws SQLException {
    // SQL statement for update
    String sql = "update \"EmotionsTrackingWebsite\".users set username=?, password= ?, email= ?, updated_at= now() where id=?;";

    // Mock Connection and PreparedStatement
    when(mockConnection.prepareStatement(sql)).thenReturn(mockStatement);
    when(mockStatement.executeUpdate()).thenReturn(1);

    // Mock ResultSet for the GetSingle method
    ResultSet mockResultSet = mock(ResultSet.class);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);  // Simulating a result being returned

    // Simulate ResultSet returning values when getXXX methods are called
    when(mockResultSet.getString("username")).thenReturn("UpdatedUser");
    when(mockResultSet.getString("password")).thenReturn("UpdatedPass");
    when(mockResultSet.getString("email")).thenReturn("updated@test.com");
    when(mockResultSet.getInt("id")).thenReturn(1);

    // Create user object to update
    User userToUpdate = new User(1, "UpdatedUser", "UpdatedPass", "updated@test.com", null, null, 0);

    // Call the update method
    usersDAODB.Update(userToUpdate);

    // Verify interactions
    verify(mockStatement).setString(1, "UpdatedUser");
    verify(mockStatement).setString(2, "UpdatedPass");
    verify(mockStatement).setString(3, "updated@test.com");
    verify(mockStatement).setInt(4, 1);
    verify(mockStatement).executeUpdate();
  }


}
