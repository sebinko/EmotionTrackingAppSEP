package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class ReactionsDAODB implements ReactionsDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(ReactionsDAODB.class.getName());

  @Inject
  public ReactionsDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public Reaction Create(Reaction reaction) {
    Connection connection = connector.getConnection();
    String sql = "insert into \"EmotionsTrackingWebsite\".reactions (user_id, emotion_checkin_id, emoji) " +
        "values (?, ?, ?) returning *;";

    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, reaction.getUserId());
      statement.setInt(2, reaction.getEmotionCheckinId());
      statement.setString(3, reaction.getEmoji());

      ResultSet resultSet = statement.executeQuery();
      if (resultSet.next()) {
        reaction.setUserId(resultSet.getInt("user_id"));
        reaction.setEmotionCheckinId(resultSet.getInt("emotion_checkin_id"));
        reaction.setEmoji(resultSet.getString("emoji"));
        reaction.setCreatedAt(resultSet.getString("created_at"));
        reaction.setUpdatedAt(resultSet.getString("updated_at"));
      }
    } catch (Exception e) {
      throw new RuntimeException(e);
    }

    return reaction;
  }
}
