package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;

public class EmotionCheckInsDAODB implements EmotionCheckInsDAO{

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
    String sql = "select * from \"EmotionsTrackingWebsite\".emotionCheckIns where id = ?;";
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
      logger.error(e.getMessage());
    }
    return emotionCheckIn;
  }

  @Override
  public EmotionCheckIn Delete(EmotionCheckIn emotionCheckIn) {
    Connection connection = connector.getConnection();

    EmotionCheckIn emotionCheckInToUpdate = GetSingle(emotionCheckIn.getId());
    if (emotionCheckInToUpdate == null) {
      throw new RuntimeException("EmotionCheckIn not found");
    }
    String sql = "delete from \"EmotionsTrackingWebsite\".emotionCheckIns where id= ?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, emotionCheckIn.getId());
      statement.executeUpdate();
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return emotionCheckIn;
  }
}
