package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Friendship;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.runner.RunWith;
import org.mockito.junit.MockitoJUnitRunner;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.HashMap;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@RunWith(MockitoJUnitRunner.class)
public class UserFriendsDAODBTest {

  private DBConnector mockDBConnector;
  private Connection mockConnection;
  private PreparedStatement mockPreparedStatement;
  private ResultSet mockResultSet;
  private UsersDAO mockUsersDAO;
  private UserFriendsDAODB userFriendsDAODB;

  @BeforeEach
  void setUp() throws SQLException {
    mockDBConnector = mock(DBConnector.class);
    mockConnection = mock(Connection.class);
    mockPreparedStatement = mock(PreparedStatement.class);
    mockResultSet = mock(ResultSet.class);
    mockUsersDAO = mock(UsersDAO.class);

    when(mockDBConnector.getConnection()).thenReturn(mockConnection);
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockPreparedStatement);

    userFriendsDAODB = new UserFriendsDAODB(mockDBConnector, mockUsersDAO);
  }

  @Test
  void testCreateFriendship() throws SQLException {
    // Arrange: Mock the connection, statement, and execute method properly
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockPreparedStatement);

    // Ensure execute() is mocked as a void method
    doNothing().when(mockPreparedStatement).execute();

    // Act: Call the method under test
    userFriendsDAODB.CreateFriendship(1, 2);

    // Assert: Verify the expected behaviors on the PreparedStatement
    verify(mockPreparedStatement, times(1)).setInt(1, 1);  // Verify that the correct userId was set
    verify(mockPreparedStatement, times(1)).setInt(2, 2);  // Verify that the correct friendId was set
    verify(mockPreparedStatement, times(1)).execute();     // Ensure execute() was called once
  }





  @Test
  void testGetFriendship() throws SQLException {
    // Arrange
    when(mockPreparedStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("user_id")).thenReturn(1);
    when(mockResultSet.getInt("friend_id")).thenReturn(2);
    when(mockResultSet.getBoolean("is_accepted")).thenReturn(true);

    // Act
    Friendship friendship = userFriendsDAODB.GetFriendShip(1, 2);

    // Assert
    assertNotNull(friendship);
    assertEquals(1, friendship.getUser1Id());
    assertEquals(2, friendship.getUser2Id());
    assertTrue(friendship.isAccepted());
  }

  @Test
  void testRemoveFriendship() throws SQLException {
    // Arrange
    when(mockPreparedStatement.executeUpdate()).thenReturn(1); // Simulate one row affected by the update

    // Act
    userFriendsDAODB.RemoveFriendship(1, 2);

    // Assert
    verify(mockPreparedStatement, times(1)).setInt(1, 1);  // Verify setInt for user1Id
    verify(mockPreparedStatement, times(1)).setInt(2, 2);  // Verify setInt for user2Id
    verify(mockPreparedStatement, times(1)).setInt(3, 2);  // Verify setInt for friend2Id
    verify(mockPreparedStatement, times(1)).setInt(4, 1);  // Verify setInt for friend1Id
    verify(mockPreparedStatement, times(1)).executeUpdate(); // Verify executeUpdate is called
  }


  @Test
  void testUpdateFriendship() throws SQLException {
    // Arrange
    Friendship friendship = new Friendship(1, 2, true);

    // Use when() instead of doNothing() to mock executeUpdate's return value
    when(mockPreparedStatement.executeUpdate()).thenReturn(1); // Simulate successful update (one row affected)

    // Act
    Friendship updatedFriendship = userFriendsDAODB.UpdateFriendship(friendship);

    // Assert
    verify(mockPreparedStatement, times(1)).setBoolean(1, true);  // Verify setBoolean
    verify(mockPreparedStatement, times(1)).setInt(2, 1);         // Verify setInt for user1Id
    verify(mockPreparedStatement, times(1)).setInt(3, 2);         // Verify setInt for user2Id
    verify(mockPreparedStatement, times(1)).setInt(4, 2);         // Verify setInt for friend2Id
    verify(mockPreparedStatement, times(1)).setInt(5, 1);         // Verify setInt for friend1Id
    verify(mockPreparedStatement, times(1)).executeUpdate();      // Ensure executeUpdate was called
    assertEquals(friendship, updatedFriendship);                   // Assert the returned object matches the input
  }


  @Test
  public void testGetFriendsWithCheckIn() throws SQLException {
    // Simulate primary query (retrieve friends)
    PreparedStatement mockStatement1 = mock(PreparedStatement.class);
    ResultSet mockResultSet1 = mock(ResultSet.class);

    // Mock behavior for first query
    when(mockConnection.prepareStatement("SELECT * FROM friends WHERE user_id = ?")).thenReturn(mockStatement1);
    when(mockStatement1.executeQuery()).thenReturn(mockResultSet1);

    // Simulate friends retrieval (one friend in database)
    when(mockResultSet1.next()).thenReturn(true, false);  // First returns true (one friend), second false (end of result set)
    when(mockResultSet1.getInt("id")).thenReturn(2);
    when(mockResultSet1.getString("username")).thenReturn("friend1");
    when(mockResultSet1.getString("email")).thenReturn("friend1@example.com");
    when(mockResultSet1.getTimestamp("created_at")).thenReturn(new Timestamp(System.currentTimeMillis()));
    when(mockResultSet1.getTimestamp("updated_at")).thenReturn(new Timestamp(System.currentTimeMillis()));
    when(mockResultSet1.getInt("current_streak")).thenReturn(5);

    // Simulate second query for check-ins
    PreparedStatement mockStatement2 = mock(PreparedStatement.class);
    ResultSet mockResultSet2 = mock(ResultSet.class);

    // Mock behavior for second query
    when(mockConnection.prepareStatement("SELECT emotion FROM check_ins WHERE user_id = ?")).thenReturn(mockStatement2);
    when(mockStatement2.executeQuery()).thenReturn(mockResultSet2);

    // Simulate check-in retrieval (one record found with emotion 'happy')
    when(mockResultSet2.next()).thenReturn(true);  // Simulate one record (check-in)
    when(mockResultSet2.getString("emotion")).thenReturn("happy");

    // Act
    HashMap<User, String> result = userFriendsDAODB.GetFriendsWithCheckIn(1);

    // Assert
    assertEquals(1, result.size());  // Ensure there's exactly one friend in the result
    User user = result.keySet().iterator().next();
    assertEquals("friend1", user.getUsername());  // Ensure username is correct
    assertEquals("happy", result.get(user));     // Ensure emotion is correct

    // Verify queries
    verify(mockConnection).prepareStatement("SELECT * FROM friends WHERE user_id = ?");
    verify(mockStatement1).executeQuery();
    verify(mockConnection).prepareStatement("SELECT emotion FROM check_ins WHERE user_id = ?");
    verify(mockStatement2).executeQuery();
  }


}
