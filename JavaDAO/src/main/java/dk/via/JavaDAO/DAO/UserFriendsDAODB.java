package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Friendship;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class UserFriendsDAODB implements UserFriendsDAO {

  private final DBConnector connector;
  private final UsersDAO userDAO;

  @Inject
  public UserFriendsDAODB(DBConnector connector, UsersDAO userDAO) {
    this.connector = connector;
    this.userDAO = userDAO;
  }

  @Override
  public void CreateFriendship(Integer user1Id, Integer user2Id) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "insert into \"EmotionsTrackingWebsite\".user_friends (user_id, friend_id)  values (?,?) returning *;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, user1Id);
    statement.setInt(2, user2Id);

    statement.execute();
  }

  @Override
  public HashMap<User, EmotionCheckIn> GetFriendsWithCheckIn(Integer userId) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "SELECT * FROM \"EmotionsTrackingWebsite\".users_with_streaks WHERE id IN " +
        "(SELECT friend_id FROM \"EmotionsTrackingWebsite\".user_friends WHERE " +
        "(user_id = ? AND is_accepted = true) " +
        "UNION " +
        "SELECT user_id FROM \"EmotionsTrackingWebsite\".user_friends WHERE " +
        "(friend_id = ? AND is_accepted = true)) " +
        "AND id != ? " +
        "ORDER BY id;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, userId);
    statement.setInt(2, userId);
    statement.setInt(3, userId);

    ResultSet resultSet = statement.executeQuery();

    List<User> users = new ArrayList<>();

    while (resultSet.next()) {
      users.add(new User(resultSet.getInt("id"), resultSet.getString("username"),
          resultSet.getString("email"), resultSet.getTimestamp("created_at"),
          resultSet.getTimestamp("updated_at"), resultSet.getInt("current_streak")));
    }

    HashMap<User, EmotionCheckIn> friendsWithCheckIn = new HashMap<>();

    for (User user : users) {
      String sql2 = "SELECT * FROM \"EmotionsTrackingWebsite\".emotion_checkins WHERE user_id = ? ORDER BY created_at DESC LIMIT 1;";
      PreparedStatement statement2 = connection.prepareStatement(sql2);

      statement2.setInt(1, user.getId());

      ResultSet resultSet2 = statement2.executeQuery();

      if (resultSet2.next()) {
        EmotionCheckIn emotionCheckIn = new EmotionCheckIn(
            resultSet2.getInt("id"),
            resultSet2.getString("emotion"),
            resultSet2.getString("description"),
            resultSet2.getTimestamp("created_at"),
            resultSet2.getTimestamp("updated_at"),
            resultSet2.getInt("user_id")
        );
        friendsWithCheckIn.put(user, emotionCheckIn);
      } else {
        friendsWithCheckIn.put(user, null);
      }
    }

    return friendsWithCheckIn;
  }

  @Override
  public void RemoveFriendship(Integer user1Id, Integer user2Id) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".user_friends WHERE ((user_id = ? AND friend_id = ?) OR (user_id = ? AND friend_id = ?));";
    try (PreparedStatement statement = connection.prepareStatement(sql)) {
      statement.setInt(1, user1Id);
      statement.setInt(2, user2Id);
      statement.setInt(3, user2Id);
      statement.setInt(4, user1Id);
      statement.executeUpdate();
    }
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
