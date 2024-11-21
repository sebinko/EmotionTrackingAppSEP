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
        emotionCheckIn = new EmotionCheckIn(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            Integer.parseInt(resultSet.getObject(5).toString())
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
        EmotionCheckIn emotionCheckIn = new EmotionCheckIn(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            Integer.parseInt(resultSet.getObject(5).toString())
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
    String sql = "insert into \"EmotionsTrackingWebsite\".emotion_checkins (emotion, user_id)  values (?, ?) returning *;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotionCheckIn.getEmotion());
      statement.setInt(2, emotionCheckIn.getUserId());

      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        emotionCheckIn.setId(Integer.parseInt(resultSet.getObject(1).toString()));
        emotionCheckIn.setEmotion(resultSet.getObject(2).toString());
        emotionCheckIn.setCreatedAt(resultSet.getObject(3).toString());
        emotionCheckIn.setUpdatedAt(resultSet.getObject(4).toString());
        emotionCheckIn.setUserId(Integer.parseInt(resultSet.getObject(5).toString()));
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
    String sql = "update \"EmotionsTrackingWebsite\".emotion_checkins set emotion= ?, updated_at= now() where id=?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotion.getEmotion());
      statement.setInt(2, emotion.getId());
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
