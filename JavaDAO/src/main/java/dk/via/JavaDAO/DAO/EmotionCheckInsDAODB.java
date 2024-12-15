package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import org.postgresql.util.PSQLState;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class EmotionCheckInsDAODB implements EmotionCheckInsDAO {

  private final DBConnector connector;
  private final TagsDAO tagsDAO;
  private final Logger logger = LoggerFactory.getLogger(EmotionCheckInsDAODB.class.getName());

  @Inject
  public EmotionCheckInsDAODB(DBConnector connector, TagsDAO tagsDAO) {
    super();
    this.connector = connector;
    this.tagsDAO = tagsDAO;
  }

  @Override
  public EmotionCheckIn GetSingle(int id) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".emotion_checkins where id = ?;";
    EmotionCheckIn emotionCheckIn = null;

      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        emotionCheckIn = new EmotionCheckIn(
            resultSet.getInt("id"),
            resultSet.getString("emotion"),
            resultSet.getString("description"),
            resultSet.getString("created_at"),
            resultSet.getString("updated_at"),
            resultSet.getInt("user_id")
        );

      }

    return emotionCheckIn;
  }

  @Override
  public ArrayList<EmotionCheckIn> GetAll(int userId) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".emotion_checkins where user_id = ?;";
    ArrayList<EmotionCheckIn> emotionCheckIns = new ArrayList<>();

      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, userId);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        String description =
            resultSet.getObject(3) != null ? resultSet.getObject(3).toString() : "";
        EmotionCheckIn emotionCheckIn = new EmotionCheckIn(
            resultSet.getInt("id"),
            resultSet.getString("emotion"),
            resultSet.getString("description"),
            resultSet.getString("created_at"),
            resultSet.getString("updated_at"),
            resultSet.getInt("user_id")
        );
        emotionCheckIns.add(emotionCheckIn);
      }

    return emotionCheckIns;
  }

  @Override
  public EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, List<Tag> tags)
      throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "insert into \"EmotionsTrackingWebsite\".emotion_checkins (emotion, user_id, description)  values (?, ?, ?) returning *;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, emotionCheckIn.getEmotion());
    statement.setInt(2, emotionCheckIn.getUserId());
    statement.setString(3, emotionCheckIn.getDescription());

    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      emotionCheckIn.setId(resultSet.getInt("id"));
      emotionCheckIn.setEmotion(resultSet.getString("emotion"));
      emotionCheckIn.setDescription(resultSet.getString("description"));
      emotionCheckIn.setCreatedAt(resultSet.getString("created_at"));
      emotionCheckIn.setUpdatedAt(resultSet.getString("updated_at"));
      emotionCheckIn.setUserId(resultSet.getInt("user_id"));
    }

    if (tags != null) {
      for (Tag tag : tags) {
        tagsDAO.AssignTag(tag, emotionCheckIn);
      }
    }

    return emotionCheckIn;
  }

  @Override
  public EmotionCheckIn Update(EmotionCheckIn emotion, List<Tag> tags) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "update \"EmotionsTrackingWebsite\".emotion_checkins set emotion= ?, description= ?, updated_at= now() where id=?";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, emotion.getEmotion());
    statement.setString(2, emotion.getDescription());
    statement.setInt(3, emotion.getId());
    statement.executeUpdate();

    if (tags != null) {
      List<Tag> existingTags = tagsDAO.GetAllForCheckIn(emotion);

      for (Tag existingTag : existingTags) {
        if (!tags.contains(existingTag)) {
          tagsDAO.RemoveTag(existingTag, emotion);
        }
      }

      for (Tag tag : tags) {
        tagsDAO.AssignTag(tag, emotion);
      }

    }

    return GetSingle(emotion.getId());
  }

  @Override
  public void Delete(int id) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "delete from \"EmotionsTrackingWebsite\".emotion_checkins where id= ?;";
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      statement.executeUpdate();
  }
}
