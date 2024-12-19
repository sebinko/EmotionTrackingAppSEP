package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;

import java.sql.*;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class ReactionsDAODBTest {

  private ReactionsDAODB reactionsDAO;
  private DBConnector mockConnector;
  private Connection mockConnection;
  private PreparedStatement mockStatement;
  private ResultSet mockResultSet;

  @BeforeEach
  void setUp() throws SQLException {
    mockConnector = mock(DBConnector.class);
    mockConnection = mock(Connection.class);
    mockStatement = mock(PreparedStatement.class);
    mockResultSet = mock(ResultSet.class);

    // Set up the connection behavior
    when(mockConnector.getConnection()).thenReturn(mockConnection);
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);

    reactionsDAO = new ReactionsDAODB(mockConnector);
  }

  @Test
  void testCreate_ShouldReturnReaction() throws SQLException {
    // Arrange
    Reaction reaction = new Reaction();
    reaction.setUserId(1);
    reaction.setEmotionCheckinId(2);
    reaction.setEmoji("😊");

    // Simulate the behavior of ResultSet
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("user_id")).thenReturn(1);
    when(mockResultSet.getInt("emotion_checkin_id")).thenReturn(2);
    when(mockResultSet.getString("emoji")).thenReturn("😊");
    when(mockResultSet.getTimestamp("created_at")).thenReturn(Timestamp.valueOf("2024-06-09 12:00:00"));
    when(mockResultSet.getTimestamp("updated_at")).thenReturn(Timestamp.valueOf("2024-06-09 12:00:00"));

    // Act
    Reaction result = reactionsDAO.Create(reaction);

    // Assert
    assertNotNull(result);
    assertEquals(1, result.getUserId());
    assertEquals(2, result.getEmotionCheckinId());
    assertEquals("😊", result.getEmoji());
    assertEquals(Timestamp.valueOf("2024-06-09 12:00:00"), result.getCreatedAt());
    verify(mockStatement, times(1)).executeQuery();
  }

  @Test
  void testCreate_WhenNoResult_ShouldReturnOriginalReaction() throws SQLException {
    // Arrange
    Reaction reaction = new Reaction();
    reaction.setUserId(1);
    reaction.setEmotionCheckinId(2);
    reaction.setEmoji("😊");

    // Simulate the behavior where no rows are returned
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(false);

    // Act
    Reaction result = reactionsDAO.Create(reaction);

    // Assert
    assertNotNull(result);
    assertEquals(1, result.getUserId());
    assertEquals(2, result.getEmotionCheckinId());
    verify(mockStatement, times(1)).executeQuery();
  }

  @Test
  void testDelete_ShouldExecuteUpdate() throws SQLException {
    // Arrange
    int userId = 1;
    int emotionCheckinId = 2;

    // Simulate execution of update
    when(mockStatement.executeUpdate()).thenReturn(1);

    // Act
    reactionsDAO.Delete(userId, emotionCheckinId);

    // Assert
    verify(mockStatement, times(1)).executeUpdate();
    verify(mockStatement, times(1)).setInt(1, userId);
    verify(mockStatement, times(1)).setInt(2, emotionCheckinId);
  }
}
