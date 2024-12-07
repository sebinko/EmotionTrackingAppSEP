package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class EmotionCheckInsDAODB implements EmotionCheckInsDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(EmotionCheckInsDAODB.class.getName());

  @Inject
  public EmotionCheckInsDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public EmotionCheckIn GetSingle(int id) {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".emotion_checkins where id = ?;";
    EmotionCheckIn emotionCheckIn = null;

    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        String description = resultSet.getObject(3) != null ? resultSet.getObject(3).toString() : "";
        emotionCheckIn = new EmotionCheckIn(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            description,
            resultSet.getObject(4).toString(),
            resultSet.getObject(5).toString(),
            Integer.parseInt(resultSet.getObject(6).toString())
        );

      }
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return emotionCheckIn;
  }

  @Override
  public ArrayList<EmotionCheckIn> GetAll(int userId) {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".emotion_checkins where user_id = ?;";
    ArrayList<EmotionCheckIn> emotionCheckIns = new ArrayList<>();

    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, userId);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        String description = resultSet.getObject(3) != null ? resultSet.getObject(3).toString() : "";
        EmotionCheckIn emotionCheckIn = new EmotionCheckIn(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            description,
            resultSet.getObject(4).toString(),
            resultSet.getObject(5).toString(),
            Integer.parseInt(resultSet.getObject(6).toString())
        );
        emotionCheckIns.add(emotionCheckIn);
      }
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return emotionCheckIns;
  }

  @Override
  public EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, ArrayList<String> tags) {
    Connection connection = connector.getConnection();
    String sql = "insert into \"EmotionsTrackingWebsite\".emotion_checkins (emotion, user_id, description)  values (?, ?, ?) returning *;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotionCheckIn.getEmotion());
      statement.setInt(2, emotionCheckIn.getUserId());
      statement.setString(3, emotionCheckIn.getDescription());

      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        String description = resultSet.getObject(3) != null ? resultSet.getObject(3).toString() : "";
        emotionCheckIn.setId(Integer.parseInt(resultSet.getObject(1).toString()));
        emotionCheckIn.setEmotion(resultSet.getObject(2).toString());
        emotionCheckIn.setDescription(description);
        emotionCheckIn.setCreatedAt(resultSet.getObject(4).toString());
        emotionCheckIn.setUpdatedAt(resultSet.getObject(5).toString());
        emotionCheckIn.setUserId(Integer.parseInt(resultSet.getObject(6).toString()));
      }
    } catch (Exception e) {
      throw new RuntimeException(e);
    }

    return emotionCheckIn;
  }

  @Override
  public EmotionCheckIn Update(EmotionCheckIn emotion, ArrayList<Tag> existingTags,
      ArrayList<String> newTags) {
    Connection connection = connector.getConnection();
    EmotionCheckIn emotionCheckInToUpdate = GetSingle(emotion.getId());
    if (emotionCheckInToUpdate == null) {
      throw new RuntimeException("Check-in not found");
    }
    String sql = "update \"EmotionsTrackingWebsite\".emotion_checkins set emotion= ?, description= ?, updated_at= now() where id=?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotion.getEmotion());
      statement.setString(2, emotion.getDescription());
      statement.setInt(3, emotion.getId());
      statement.executeUpdate();
      // TODO refetch user from db before returning
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return emotion;
  }

  @Override
  public EmotionCheckIn Delete(int id) {
    Connection connection = connector.getConnection();

    EmotionCheckIn emotionCheckInToDelete = GetSingle(id);
    if (emotionCheckInToDelete == null) {
      throw new RuntimeException("EmotionCheckIn not found");
    }
    String sql = "delete from \"EmotionsTrackingWebsite\".emotion_checkins where id= ?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      statement.executeUpdate();
    } catch (Exception e) {
      throw new RuntimeException(e);
    }

    return emotionCheckInToDelete;
  }
}
