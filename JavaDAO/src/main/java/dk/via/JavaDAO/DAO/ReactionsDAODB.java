package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
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
  public Reaction Create(Reaction reaction) throws SQLException {
    Connection connection = connector.getConnection();
    String sql =
        "insert into \"EmotionsTrackingWebsite\".reactions (user_id, emotion_checkin_id, emoji) " +
            "values (?, ?, ?) returning *;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, reaction.getUserId());
    statement.setInt(2, reaction.getEmotionCheckinId());
    statement.setString(3, reaction.getEmoji());

    ResultSet resultSet = statement.executeQuery();
    if (resultSet.next()) {
      reaction.setUserId(resultSet.getInt("user_id"));
      reaction.setEmotionCheckinId(resultSet.getInt("emotion_checkin_id"));
      reaction.setEmoji(resultSet.getString("emoji"));
      reaction.setCreatedAt(resultSet.getTimestamp("created_at"));
      reaction.setUpdatedAt(resultSet.getTimestamp("updated_at"));
    }

    return reaction;
  }

  @Override
  public void Delete(Integer userId, Integer emotionCheckinId)
      throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "delete from \"EmotionsTrackingWebsite\".reactions where user_id = ? and emotion_checkin_id = ?;";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, userId);
    statement.setInt(2, emotionCheckinId);
    statement.executeUpdate();

  }

  @Override
  public List<Reaction> GetReactionsForEmotionCheckIn(Integer emotionCheckInId)
      throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "select * from \"EmotionsTrackingWebsite\".reactions where emotion_checkin_id = ?;";

    PreparedStatement statement = connection.prepareStatement(sql);

    statement.setInt(1, emotionCheckInId);

    ResultSet resultSet = statement.executeQuery();

    List<Reaction> reactions = new ArrayList<>();

    while (resultSet.next()) {
      reactions.add(new Reaction(
          resultSet.getInt("user_id"),
          resultSet.getInt("emotion_checkin_id"),
          resultSet.getString("emoji"),
          resultSet.getTimestamp("created_at"),
          resultSet.getTimestamp("updated_at")
      ));
    }

    return reactions;
  }
}
