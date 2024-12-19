package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Util.AppConfig;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Util.PostgresConnector;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestInstance;
import java.sql.Connection;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
public class EmotionCheckInsDAODBIntegrationTests {

  private EmotionCheckInsDAO emotionCheckInsDAO;
  private DBConnector dbConnector;
  private Connection connection;

  @BeforeAll
  void setUp() throws SQLException {
    AppConfig appConfig = new AppConfig();
    dbConnector = new PostgresConnector(appConfig);
    emotionCheckInsDAO = new EmotionCheckInsDAODB(dbConnector, new TagsDAODB(dbConnector));
    connection = dbConnector.getConnection();
  }

  @Test
  void testGetSingle() throws SQLException {
    EmotionCheckIn emotionCheckIn = new EmotionCheckIn(1, "Accomplished", null, null, null, 1);
    emotionCheckIn = emotionCheckInsDAO.Create(emotionCheckIn, new ArrayList<>());

    EmotionCheckIn result = emotionCheckInsDAO.GetSingle(emotionCheckIn.getId());

    assertNotNull(result);
    assertEquals(emotionCheckIn.getId(), result.getId());
    assertEquals("Accomplished", result.getEmotion());
    assertEquals(null, result.getDescription());
  }

  @Test
  void testGetAll() throws SQLException {
    int userId = 1;
    ArrayList<EmotionCheckIn> result = emotionCheckInsDAO.GetAll(userId);

    assertNotNull(result);
    assertFalse(result.isEmpty());
  }

  @Test
  void testCreate() throws SQLException {
    EmotionCheckIn emotionCheckIn = new EmotionCheckIn(1, "happy", "Feeling good", null, null, 1);
    List<Tag> tags = new ArrayList<>();

    EmotionCheckIn result = emotionCheckInsDAO.Create(emotionCheckIn, tags);

    assertNotNull(result);
    assertTrue(result.getId() > 0);
    assertEquals("happy", result.getEmotion());
  }

  @Test
  void testUpdate() throws SQLException {
    EmotionCheckIn emotionCheckIn = new EmotionCheckIn(1, "happy", "Feeling good", null, null, 1);
    emotionCheckIn = emotionCheckInsDAO.Create(emotionCheckIn, new ArrayList<>());

    emotionCheckIn.setEmotion("excited");
    EmotionCheckIn result = emotionCheckInsDAO.Update(emotionCheckIn, new ArrayList<>());

    assertNotNull(result);
    assertEquals("excited", result.getEmotion());
  }

  @Test
  void testDelete() throws SQLException {
    EmotionCheckIn emotionCheckIn = new EmotionCheckIn(1, "happy", "Feeling good", null, null, 1);
    emotionCheckIn = emotionCheckInsDAO.Create(emotionCheckIn, new ArrayList<>());

    final int emotionCheckInId = emotionCheckIn.getId();

    assertDoesNotThrow(() -> emotionCheckInsDAO.Delete(emotionCheckInId));

    EmotionCheckIn result = emotionCheckInsDAO.GetSingle(emotionCheckInId);
    assertNull(result);
  }

  @Test
  void testGetSingleNotExisting() throws SQLException {
    EmotionCheckIn result = emotionCheckInsDAO.GetSingle(9999);
    assertNull(result);
  }

  @Test
  void testCreateWithInvalidData() {
    EmotionCheckIn invalidEmotionCheckIn = new EmotionCheckIn(1, null, null, null, null, 1);
    assertThrows(SQLException.class, () -> {
      emotionCheckInsDAO.Create(invalidEmotionCheckIn, new ArrayList<>());
    });
  }

  @Test
  void testUpdateWithInvalidData() throws SQLException {
    EmotionCheckIn emotionCheckIn = new EmotionCheckIn(1, "happy", "Feeling good", null, null, 1);
    emotionCheckIn = emotionCheckInsDAO.Create(emotionCheckIn, new ArrayList<>());

    EmotionCheckIn updatedEmotionCheckIn = new EmotionCheckIn(emotionCheckIn.getId(), null, "Feeling good", null, null, 1);
    assertThrows(SQLException.class, () -> {
      emotionCheckInsDAO.Update(updatedEmotionCheckIn, new ArrayList<>());
    });
  }

  @Test
  void testDeleteWithInvalidId() throws SQLException {
    int invalidId = 9999;
    emotionCheckInsDAO.Delete(invalidId);
    EmotionCheckIn result = emotionCheckInsDAO.GetSingle(invalidId);
    assertNull(result);
  }
}
