package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.*;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;

import org.junit.jupiter.api.*;
import org.mockito.Mockito;

import java.sql.*;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class TagsDAODBTest {

  private DBConnector mockConnector;
  private Connection mockConnection;
  private PreparedStatement mockStatement;
  private ResultSet mockResultSet;
  private TagsDAODB tagsDAODB;

  @BeforeEach
  void setUp() throws SQLException {
    mockConnector = mock(DBConnector.class);
    mockConnection = mock(Connection.class);
    mockStatement = mock(PreparedStatement.class);
    mockResultSet = mock(ResultSet.class);

    when(mockConnector.getConnection()).thenReturn(mockConnection);
    tagsDAODB = new TagsDAODB(mockConnector);
  }

  @Test
  void testGetSingle_ById() throws SQLException {
    int tagId = 1;
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("id")).thenReturn(tagId);
    when(mockResultSet.getString("key")).thenReturn("TestTag");
    when(mockResultSet.getString("type")).thenReturn("WHAT");
    when(mockResultSet.getInt("user_id")).thenReturn(10);

    Tag result = tagsDAODB.GetSingle(tagId);

    assertNotNull(result);
    assertEquals(tagId, result.getId());
    assertEquals("TestTag", result.getKey());
    assertEquals(TagType.WHAT, result.getType());
    assertEquals(10, result.getUserId());

    verify(mockConnection).prepareStatement("select * from \"EmotionsTrackingWebsite\".tags where id = ?;");
    verify(mockStatement).setInt(1, tagId);
    verify(mockStatement).executeQuery();
    verify(mockResultSet).next();
  }

  @Test
  void testGetSingle_ByKeyTypeUserId() throws SQLException {
    String key = "TestTag";
    TagType type = TagType.WITH;
    int userId = 5;

    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("id")).thenReturn(2);
    when(mockResultSet.getString("key")).thenReturn(key);
    when(mockResultSet.getString("type")).thenReturn(type.toString());
    when(mockResultSet.getInt("user_id")).thenReturn(userId);

    Tag result = tagsDAODB.GetSingle(key, type, userId);

    assertNotNull(result);
    assertEquals("TestTag", result.getKey());
    assertEquals(TagType.WITH, result.getType());
    assertEquals(5, result.getUserId());

    verify(mockStatement).setString(1, key);
    verify(mockStatement).setObject(2, type.toString(), java.sql.Types.OTHER);
    verify(mockStatement).setInt(3, userId);
  }

  @Test
  void testGetAllForCheckIn() throws SQLException {
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Felt good", "now", "later", 10);

    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true, true, false); // 2 tags found
    when(mockResultSet.getInt("id")).thenReturn(1, 2);
    when(mockResultSet.getString("key")).thenReturn("Tag1", "Tag2");
    when(mockResultSet.getString("type")).thenReturn("WHAT", "WHERE");
    when(mockResultSet.getInt("user_id")).thenReturn(10, 10);

    List<Tag> tags = tagsDAODB.GetAllForCheckIn(checkIn);

    assertEquals(2, tags.size());
    assertEquals("Tag1", tags.get(0).getKey());
    assertEquals(TagType.WHAT, tags.get(0).getType());
    assertEquals("Tag2", tags.get(1).getKey());
    assertEquals(TagType.WHERE, tags.get(1).getType());

    verify(mockStatement).setInt(1, checkIn.getId());
  }

  @Test
  void testAssignTag() throws SQLException {
    // Assuming TagType is required to be non-null:
    Tag tag = new Tag(2, "NewTag", TagType.WHERE, null, null, 10);  // Ensure non-null tagType (e.g., WHERE)
    EmotionCheckIn checkIn = new EmotionCheckIn(1, "Happy", "Test", "now", "later", 10);

    // Mocks for tag insertion
    when(mockConnection.prepareStatement(anyString())).thenReturn(mockStatement);
    when(mockStatement.executeQuery()).thenReturn(mockResultSet);
    when(mockResultSet.next()).thenReturn(true);
    when(mockResultSet.getInt("id")).thenReturn(2);

    // Act
    tagsDAODB.AssignTag(tag, checkIn);

    // Verify that mock set methods are called correctly
    verify(mockStatement).setString(1, "NewTag");
    verify(mockStatement).setInt(2, 10);
    verify(mockStatement).setObject(3, TagType.WHERE.toString(), java.sql.Types.OTHER);

    // Verify second query execution (tag_emotions insertion)
    verify(mockStatement, atLeastOnce()).executeUpdate();
  }

}
