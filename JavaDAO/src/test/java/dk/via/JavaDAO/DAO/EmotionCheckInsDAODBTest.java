package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.DAO.EmotionCheckInsDAODB;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Models.TagType;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;

import java.sql.*;
import java.util.*;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class EmotionCheckInsDAODBTest {

  private EmotionCheckInsDAODB emotionCheckInsDAO;

  @Mock private DBConnector connector;
  @Mock private Connection connection;
  @Mock private PreparedStatement statement;
  @Mock private ResultSet resultSet;
  @Mock private TagsDAO tagsDAO;

  @BeforeEach
  void setUp() throws Exception {
    MockitoAnnotations.openMocks(this);

    when(connector.getConnection()).thenReturn(connection);
    when(connection.prepareStatement(any(String.class))).thenReturn(statement);
    when(statement.executeQuery()).thenReturn(resultSet);

    emotionCheckInsDAO = new EmotionCheckInsDAODB(connector, tagsDAO);
  }

  // Test for GetSingle()
  @Test
  void testGetSingle_Success() throws Exception {
    // Arrange
    int checkInId = 1;
    when(resultSet.next()).thenReturn(true).thenReturn(false); // Single record
    when(resultSet.getInt("id")).thenReturn(checkInId);
    when(resultSet.getString("emotion")).thenReturn("Happy");
    when(resultSet.getString("description")).thenReturn("Feeling joyful");
    when(resultSet.getString("created_at")).thenReturn("2024-06-01");
    when(resultSet.getString("updated_at")).thenReturn("2024-06-01");
    when(resultSet.getInt("user_id")).thenReturn(123);

    // Act
    EmotionCheckIn result = emotionCheckInsDAO.GetSingle(checkInId);

    // Assert
    assertNotNull(result);
    assertEquals(1, result.getId());
    assertEquals("Happy", result.getEmotion());
    assertEquals("Feeling joyful", result.getDescription());
    verify(statement, times(1)).setInt(1, checkInId);
  }

  // Test for GetAll()
  @Test
  void testGetAll_Success() throws Exception {
    // Arrange
    int userId = 123;
    when(resultSet.next()).thenReturn(true, true, false); // 2 rows returned
    when(resultSet.getInt("id")).thenReturn(1, 2);
    when(resultSet.getString("emotion")).thenReturn("Happy", "Sad");
    when(resultSet.getString("description")).thenReturn("Joyful", "Low");
    when(resultSet.getString("created_at")).thenReturn("2024-06-01", "2024-06-02");
    when(resultSet.getString("updated_at")).thenReturn("2024-06-01", "2024-06-02");

    // Act
    List<EmotionCheckIn> result = emotionCheckInsDAO.GetAll(userId);

    // Assert
    assertNotNull(result);
    assertEquals(2, result.size());
    verify(statement, times(1)).setInt(1, userId);
    verify(statement, times(1)).executeQuery();
  }

  // Test for Create()
  @Test
  void testCreate_Success() throws Exception {
    // Arrange
    EmotionCheckIn newCheckIn = new EmotionCheckIn(0, "Excited", "Ready for the big day", null, null, 123);
    Tag sampleTag = new Tag(1, "key", TagType.WHAT, null, null, 1);

    // Mocked connections and statements
    when(connector.getConnection()).thenReturn(connection);
    when(connection.prepareStatement(anyString())).thenReturn(statement);
    when(statement.executeQuery()).thenReturn(resultSet);

    // Simulate returned data from resultSet
    when(resultSet.next()).thenReturn(true).thenReturn(false); // 1 iteration
    when(resultSet.getInt("id")).thenReturn(1);
    when(resultSet.getString("emotion")).thenReturn("Excited");
    when(resultSet.getString("description")).thenReturn("Ready for the big day");
    when(resultSet.getString("created_at")).thenReturn("2024-06-03");
    when(resultSet.getString("updated_at")).thenReturn("2024-06-03");
    when(resultSet.getInt("user_id")).thenReturn(123);

    // Act
    EmotionCheckIn result = emotionCheckInsDAO.Create(newCheckIn, List.of(sampleTag));

    // Assert
    assertEquals(1, result.getId());
    assertEquals("Excited", result.getEmotion());
    assertEquals("Ready for the big day", result.getDescription());

    // Verify tag assignment
    verify(tagsDAO, times(1)).AssignTag(sampleTag, result);

    // Ensure `next()` is called only once.
    verify(resultSet, times(2)).next();
    verify(statement, times(1)).executeQuery();
    verify(connection, times(1)).prepareStatement(anyString());
  }



  @Test
  void testUpdate_Success() throws Exception {
    // Arrange
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Calm", "Relaxing day", null, null, 123);
    Tag existingTag = new Tag(1, "key", TagType.WHAT, null, null, 1);
    Tag newTag = new Tag(2, "newKey", TagType.WITH, null, null, 1);

    // Mock DAO and database connection setup
    when(connector.getConnection()).thenReturn(connection);
    when(connection.prepareStatement(anyString())).thenReturn(statement);
    when(statement.executeUpdate()).thenReturn(1);
    when(tagsDAO.GetAllForCheckIn(checkIn)).thenReturn(List.of(existingTag));

    // Simulating fetching the updated EmotionCheckIn
    when(statement.executeQuery()).thenReturn(resultSet);
    when(resultSet.next()).thenReturn(true).thenReturn(false); // Ensure it stops after one iteration
    when(resultSet.getInt("id")).thenReturn(1);
    when(resultSet.getString("emotion")).thenReturn("Calm");
    when(resultSet.getString("description")).thenReturn("Relaxing day");
    when(resultSet.getString("updated_at")).thenReturn("2024-06-03");

    // Act
    EmotionCheckIn result = emotionCheckInsDAO.Update(checkIn, List.of(newTag));

    // Assert
    assertNotNull(result);
    assertEquals("Calm", result.getEmotion());
    assertEquals("Relaxing day", result.getDescription());

    // Verify tag interactions
    verify(tagsDAO, times(1)).RemoveTag(existingTag, checkIn);
    verify(tagsDAO, times(1)).AssignTag(newTag, checkIn);

    // Verify prepared statements were used
    verify(statement, times(1)).executeUpdate();
    verify(connection, times(2)).prepareStatement(anyString());
  }


  // Test for Delete()
  @Test
  void testDelete_Success() throws Exception {
    // Arrange
    int checkInId = 1;

    // Act
    emotionCheckInsDAO.Delete(checkInId);

    // Assert
    verify(statement, times(1)).setInt(1, checkInId);
    verify(statement, times(1)).executeUpdate();
  }
}
