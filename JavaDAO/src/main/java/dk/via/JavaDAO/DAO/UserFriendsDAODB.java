package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.Friendship;
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

    ResultSet resultSet = fetchFriendShip(user1Id, user2Id, connection);

    boolean doesRequestExist = false;
    boolean isAccepted = false;
    int requestUserId = 0;

    while (resultSet.next()) {
      doesRequestExist = true;
      isAccepted = resultSet.getBoolean("is_accepted");
      requestUserId = resultSet.getInt("user_id");
    }

    if (!doesRequestExist) {
      createFriendshipRequest(user1Id, user2Id, connection);
      return;
    }

    if (doesRequestExist && !isAccepted && (user1Id == requestUserId)) {
      throw new SQLException("You have already sent a friend request to this user");
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

  @Override
  public void RemoveFriendship(Integer user1Id, Integer user2Id) throws SQLException {
    Connection connection = connector.getConnection();

    ResultSet resultSet = fetchFriendShip(user1Id, user2Id, connection);

    boolean doesRequestExist = false;
    while (resultSet.next()) {
      doesRequestExist = true;
    }

    if (!doesRequestExist) {
      throw new SQLException("User friendship does not exist");
    }

    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".user_friends WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    try (PreparedStatement statement = connection.prepareStatement(sql)) {
      statement.setInt(1, user1Id);
      statement.setInt(2, user2Id);
      statement.setInt(3, user2Id);
      statement.setInt(4, user1Id);
      statement.executeUpdate();
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


  private static ResultSet fetchFriendShip(Integer user1Id, Integer user2Id, Connection connection)
      throws SQLException {
    String sql = "SELECT * FROM \"EmotionsTrackingWebsite\".user_friends WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user1Id);
    statement.setInt(2, user2Id);
    statement.setInt(3, user2Id);
    statement.setInt(4, user1Id);
    ResultSet resultSet = statement.executeQuery();
    return resultSet;
  }

  @Override
  public Friendship GetFriendShip(Integer user1Id, Integer user2Id) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "SELECT * FROM \"EmotionsTrackingWebsite\".user_friends WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user1Id);
    statement.setInt(2, user2Id);
    statement.setInt(3, user2Id);
    statement.setInt(4, user1Id);

    ResultSet resultSet = statement.executeQuery();

    if (!resultSet.next()) {
      return null;
    }

    return new Friendship(resultSet.getInt("user_id"), resultSet.getInt("friend_id"),
        resultSet.getBoolean("is_accepted"));
  }

  @Override
  public Friendship UpdateFriendship(Friendship friendship) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "UPDATE \"EmotionsTrackingWebsite\".user_friends SET is_accepted = ? WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setBoolean(1, friendship.isAccepted());
    statement.setInt(2, friendship.getUser1Id());
    statement.setInt(3, friendship.getUser2Id());
    statement.setInt(4, friendship.getUser2Id());
    statement.setInt(5, friendship.getUser1Id());

    statement.executeUpdate();

    return friendship;
  }
}
