package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.Emotion;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;

public class EmotionCheckInDAODB implements EmotionCheckInsDAO{
  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(UsersDAODB.class.getName());

  @Inject
  public EmotionCheckInDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public EmotionCheckIn GetSingle(int id) {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".emotion_checkins where id = ?;";
    EmotionCheckIn emotion = null;
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        emotion = new EmotionCheckIn (
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            Integer.parseInt(resultSet.getObject(5).toString())
        );

      }
    } catch (Exception e) {
      logger.error(e.getMessage());
    }
    return emotion;


  }
  @Override
  public EmotionCheckIn Update(EmotionCheckIn emotion,ArrayList<Tag> existingTags, ArrayList<String> newTags) {
    Connection connection = connector.getConnection();

    EmotionCheckIn emotionToUpdate = GetSingle(emotion.getId());
    if (emotionToUpdate == null) {
      throw new RuntimeException("Check-in not found");
    }
    String sql = "update \"EmotionsTrackingWebsite\".emotion_checkins set emotion= ?, updated_at= now() where id=?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotion.getEmotion());
      statement.executeUpdate();

      // TODO refetch user from db before returning
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return emotion;
  }
}
