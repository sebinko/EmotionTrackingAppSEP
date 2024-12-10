package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Models.TagType;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class TagsDAODB implements TagsDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(TagsDAODB.class.getName());

  @Inject
  public TagsDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public Tag GetSingle(int id) throws SQLException {

    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".tags where id = ?;";
    Tag tag = null;

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, id);
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      tag = parseTag(resultSet);
    }

    return tag;
  }

  @Override
  public Tag GetSingle(String key, TagType type, Integer userId) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".tags where key = ? and type = ? and user_id = ?;";
    Tag tag = null;

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, key);
    statement.setObject(2, type.toString(), java.sql.Types.OTHER);
    statement.setInt(3, userId);
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      tag = parseTag(resultSet);
    }

    return tag;
  }

  @Override
  public List<Tag> GetAllForCheckIn(EmotionCheckIn emotionCheckIn) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".tags t join \"EmotionsTrackingWebsite\".tag_emotions te on t.id = te.tag_id where te.emotion_checkin_id = ?;";
    List<Tag> tags = new ArrayList<>();

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, emotionCheckIn.getId());
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      tags.add(parseTag(resultSet));
    }

    return tags;
  }

  @Override
  public List<Tag> GetAllForUser(User user) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".tags where user_id = ?;";
    List<Tag> tags = new ArrayList<>();

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user.getId());
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      tags.add(parseTag(resultSet));
    }

    return tags;
  }

  @Override
  public void AssignTag(Tag tag, EmotionCheckIn checkIn) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "INSERT INTO \"EmotionsTrackingWebsite\".tags (key, user_id, type) VALUES (?, ?, ?) ON CONFLICT (key, user_id, type) DO NOTHING RETURNING *;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, tag.getKey());
    statement.setInt(2, checkIn.getUserId());
    statement.setObject(3, tag.getType().toString(), java.sql.Types.OTHER);
    System.out.println(statement);

    ResultSet resultSet = statement.executeQuery();

    Tag newTag = null;

    if (resultSet.next()) {
      newTag = parseTag(resultSet);
    }

    if (newTag == null) {
      newTag = GetSingle(tag.getKey(), tag.getType(), checkIn.getUserId());
    }

    if (newTag == null) {
      throw new SQLException("Failed to create tag");
    }

    String sql2 = "INSERT INTO \"EmotionsTrackingWebsite\".tag_emotions (emotion_checkin_id, tag_id) VALUES (?, ?) ON CONFLICT (emotion_checkin_id, tag_id) DO NOTHING;";
    PreparedStatement statement2 = connection.prepareStatement(sql2);
    statement2.setInt(1, checkIn.getId());
    statement2.setInt(2, newTag.getId());

    statement2.executeUpdate();
  }

  @Override
  public void RemoveTag(Tag tag, EmotionCheckIn checkIn) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".tag_emotions WHERE emotion_checkin_id = ? AND tag_id = ?;";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, checkIn.getId());
    statement.setInt(2, tag.getId());

    statement.executeUpdate();
  }

  private static Tag parseTag(ResultSet resultSet) throws SQLException {
    return new Tag(
        resultSet.getInt("id"),
        resultSet.getString("key"),
        TagType.valueOf(resultSet.getString("type")),
        resultSet.getTimestamp("created_at"),
        resultSet.getTimestamp("updated_at"),
        resultSet.getInt("user_id")
    );
  }
}
