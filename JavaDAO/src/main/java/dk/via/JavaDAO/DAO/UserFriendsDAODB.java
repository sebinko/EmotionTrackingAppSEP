package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class UserFriendsDAODB implements UserFriendsDAO {

  private final DBConnector connector;

  @Inject
  public UserFriendsDAODB(DBConnector connector) {
    this.connector = connector;
  }

  @Override
  public void CreateFriendship(Integer user1Id, Integer user2Id) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "SELECT * FROM \"EmotionsTrackingWebsite\".user_friends WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user1Id);
    statement.setInt(2, user2Id);
    statement.setInt(3, user2Id);
    statement.setInt(4, user1Id);

    boolean doesRequestExist = false;
    boolean isAccepted = false;
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      doesRequestExist = true;
      isAccepted = resultSet.getBoolean("is_accepted");
    }
    
    if (!doesRequestExist) {
      createFriendshipRequest(user1Id, user2Id, connection);
      return;
    }
    
    if (!isAccepted) {
      String sqlQuery = "UPDATE \"EmotionsTrackingWebsite\".user_friends SET is_accepted = true WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
      PreparedStatement statement2 = connection.prepareStatement(sqlQuery);
      statement2.setInt(1, user1Id);
      statement2.setInt(2, user2Id);
      statement2.setInt(3, user2Id);
      statement2.setInt(4, user1Id);
      statement2.executeUpdate();
    }
    
    if (isAccepted) {
      throw new SQLException("User friendship already exists");
    }
  }

  private static void createFriendshipRequest(Integer user1Id, Integer user2Id,
      Connection connection)
      throws SQLException {
    String sql = "insert into \"EmotionsTrackingWebsite\".user_friends (user_id, friend_id)  values (?,?) returning *;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user1Id);
    statement.setInt(2, user2Id);

    statement.execute();
  }
}
