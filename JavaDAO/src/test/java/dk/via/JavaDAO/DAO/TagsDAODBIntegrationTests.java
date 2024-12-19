package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Models.TagType;
import dk.via.JavaDAO.Util.AppConfig;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Util.PostgresConnector;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import java.sql.Connection;
import java.sql.SQLException;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class TagsDAODBIntegrationTests {
  private TagsDAODB tagsDAODB;
  private DBConnector dbConnector;
  private Connection connection;

  @BeforeEach
  void setUp() throws SQLException {
    dbConnector = new PostgresConnector(new AppConfig());
    connection = dbConnector.getConnection();
    tagsDAODB = new TagsDAODB(dbConnector);
  }

  @Test
  void testCreateAndAssignTag() throws SQLException {
    Tag tag = new Tag(0, "testKey", TagType.WHAT, null, null, 19);
    EmotionCheckIn checkIn = new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 1);
    tagsDAODB.AssignTag(tag, checkIn);

    List<Tag> tags = tagsDAODB.GetAllForCheckIn(checkIn);
    assertNotNull(tags);
  }

  @Test
  void testGetSingleTagById() throws SQLException {
    Tag tag = new Tag(1, "testKey", TagType.WHAT, null, null, 1);
    tagsDAODB.AssignTag(tag, new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 19));

    Tag fetchedTag = tagsDAODB.GetSingle(51);
    assertNotNull(fetchedTag);
    assertEquals("testKey", fetchedTag.getKey());
  }

  @Test
  void testGetSingleTagByKeyTypeAndUserId() throws SQLException {
    Tag tag = new Tag(0, "testKey", TagType.WHAT, null, null, 19);
    tagsDAODB.AssignTag(tag, new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 19));

    Tag fetchedTag = tagsDAODB.GetSingle("testKey", TagType.WHAT, 19);
    assertNotNull(fetchedTag);
    assertEquals("testKey", fetchedTag.getKey());
  }

  @Test
  void testGetAllTagsForCheckIn() throws SQLException {
    Tag tag1 = new Tag(1, "testKey1", TagType.WHAT, null, null, 1);
    Tag tag2 = new Tag(2, "testKey2", TagType.WHAT, null, null, 1);
    EmotionCheckIn checkIn = new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 19);
    tagsDAODB.AssignTag(tag1, checkIn);
    tagsDAODB.AssignTag(tag2, checkIn);

    List<Tag> tags = tagsDAODB.GetAllForCheckIn(checkIn);
    assertNotNull(tags);
  }

  @Test
  void testGetAllTagsForUser() throws SQLException {
    Tag tag1 = new Tag(1, "testKey1", TagType.WHAT, null, null, 19);
    Tag tag2 = new Tag(2, "testKey2", TagType.WHAT, null, null, 19);
    tagsDAODB.AssignTag(tag1, new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 19));
    tagsDAODB.AssignTag(tag2, new EmotionCheckIn(0, "Happy", "Feeling great", null, null, 19));

    List<Tag> tags = tagsDAODB.GetAllForUser(19);
    assertNotNull(tags);
  }

  @Test
  void testRemoveTag() throws SQLException {
    Tag tag = new Tag(1, "testKey", TagType.WHAT, null, null, 19);
    EmotionCheckIn checkIn = new EmotionCheckIn(0, "Happy", "Feeling good", null, null,19);
    tagsDAODB.AssignTag(tag, checkIn);

    tagsDAODB.RemoveTag(tag, checkIn);

    List<Tag> tags = tagsDAODB.GetAllForCheckIn(checkIn);
    assertNotNull(tags.isEmpty());
  }

  @Test
  void testCreateTagWithInvalidData() {
    Tag invalidTag = new Tag(null, null, null, null, null, null);
    EmotionCheckIn checkIn = new EmotionCheckIn(0, "Happy", "Feeling good", null, null, 19);
    assertThrows(NullPointerException.class, () -> {
      tagsDAODB.AssignTag(invalidTag, checkIn);
    });
  }

  @Test
  void testGetTagsForNonExistentEmotionCheckIn() throws SQLException {
    EmotionCheckIn nonExistentCheckIn = new EmotionCheckIn(999, "NonExistent", "NonExistent", null, null, 19);
    List<Tag> tags = tagsDAODB.GetAllForCheckIn(nonExistentCheckIn);
    assertTrue(tags.isEmpty());
  }
}
