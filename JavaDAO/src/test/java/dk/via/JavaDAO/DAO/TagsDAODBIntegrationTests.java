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
  private TagsDAO tagsDAO;
  private DBConnector dbConnector;
  private Connection connection;

  @BeforeEach
  void setUp() throws SQLException {
    dbConnector = new PostgresConnector(new AppConfig());
    connection = dbConnector.getConnection();
    tagsDAO = new TagsDAODB(dbConnector);
  }

  @Test
  void testCreateAndAssignTag() throws SQLException {
    Tag tag = new Tag(1, "testKey", TagType.WHAT, null, null, 1);
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1);
    tagsDAO.AssignTag(tag, checkIn);

    List<Tag> tags = tagsDAO.GetAllForCheckIn(checkIn);
    assertNotNull(tags);
  }

  @Test
  void testGetSingleTagById() throws SQLException {
    Tag tag = new Tag(1, "Home", TagType.WHAT, null, null, 1);
    tagsDAO.AssignTag(tag, new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1));

    Tag fetchedTag = tagsDAO.GetSingle(1);
    assertNotNull(fetchedTag);
    assertEquals("Home", fetchedTag.getKey());
  }

  @Test
  void testGetSingleTagByKeyTypeAndUserId() throws SQLException {
    Tag tag = new Tag(1, "Home", TagType.WHAT, null, null, 1);
    tagsDAO.AssignTag(tag, new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1));

    Tag fetchedTag = tagsDAO.GetSingle("Home", TagType.WHAT, 1);
    assertNotNull(fetchedTag);
    assertEquals("Home", fetchedTag.getKey());
  }

  @Test
  void testGetAllTagsForCheckIn() throws SQLException {
    Tag tag1 = new Tag(1, "testKey1", TagType.WHAT, null, null, 1);
    Tag tag2 = new Tag(2, "testKey2", TagType.WHAT, null, null, 1);
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1);
    tagsDAO.AssignTag(tag1, checkIn);
    tagsDAO.AssignTag(tag2, checkIn);

    List<Tag> tags = tagsDAO.GetAllForCheckIn(checkIn);
    assertNotNull(tags);
  }

  @Test
  void testGetAllTagsForUser() throws SQLException {
    Tag tag1 = new Tag(1, "testKey1", TagType.WHAT, null, null, 1);
    Tag tag2 = new Tag(2, "testKey2", TagType.WHAT, null, null, 1);
    tagsDAO.AssignTag(tag1, new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1));
    tagsDAO.AssignTag(tag2, new EmotionCheckIn(1, "Happy", "Feeling great", null, null, 1));

    List<Tag> tags = tagsDAO.GetAllForUser(1);
    assertNotNull(tags);
  }

  @Test
  void testRemoveTag() throws SQLException {
    Tag tag = new Tag(1, "testKey", TagType.WHAT, null, null, 1);
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Feeling good", null, null,1);
    tagsDAO.AssignTag(tag, checkIn);

    tagsDAO.RemoveTag(tag, checkIn);

    List<Tag> tags = tagsDAO.GetAllForCheckIn(checkIn);
    assertNotNull(tags.isEmpty());
  }

  @Test
  void testCreateTagWithInvalidData() {
    Tag invalidTag = new Tag(null, null, null, null, null, null);
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Feeling good", null, null, 1);
    assertThrows(NullPointerException.class, () -> {
      tagsDAO.AssignTag(invalidTag, checkIn);
    });
  }

  @Test
  void testGetTagsForNonExistentEmotionCheckIn() throws SQLException {
    EmotionCheckIn nonExistentCheckIn = new EmotionCheckIn(999, "NonExistent", "NonExistent", null, null, 19);
    List<Tag> tags = tagsDAO.GetAllForCheckIn(nonExistentCheckIn);
    assertTrue(tags.isEmpty());
  }
}
