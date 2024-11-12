package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import com.google.inject.Inject;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

public class EmotionCheckInsDAODB implements EmotionCheckInsDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(EmotionCheckInsDAODB.class.getName());

  @Inject
  public EmotionCheckInsDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public EmotionCheckIn Create(EmotionCheckIn emotionCheckIn,
      ArrayList<String> tags) {
    Connection connection = connector.getConnection();
    String sql = "insert into \"EmotionsTrackingWebsite\".emotion_checkins ()  values (?) returning *;";
    try {

      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, emotionCheckIn.getEmotion());

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
}
