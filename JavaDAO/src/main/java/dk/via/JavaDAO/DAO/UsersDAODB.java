package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UsersDAODB implements UsersDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(UsersDAODB.class.getName());

  @Inject
  public UsersDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public ArrayList<User> GetAll() throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".users_with_streaks;";
    ArrayList<User> users = new ArrayList<>();
    PreparedStatement statement = connection.prepareStatement(sql);
    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      users.add(parseUserFromDB(resultSet));
    }
    return users;
  }

  @Override
  public User GetSingle(int id) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".users_with_streaks where id = ?;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setInt(1, id);
    ResultSet resultSet = statement.executeQuery();

    if (!resultSet.next()) {
      return null;
    }

    return parseUserFromDB(resultSet);
  }

  @Override
  public User GetSingle(String username, String password) throws SQLException {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".users_with_streaks where username = ? and password = ?;";

    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, username);
    statement.setString(2, password);
    ResultSet resultSet = statement.executeQuery();

    if (resultSet.next()) {
      return parseUserFromDB(resultSet);
    }

    return null;
  }

  @Override
  public User Update(User user) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "update \"EmotionsTrackingWebsite\".users set username=?, password= ?, email= ?, updated_at= now() where id=?;";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, user.getUsername());
    statement.setString(2, user.getPassword());
    statement.setString(3, user.getEmail());
    statement.setInt(4, user.getId());

    statement.executeUpdate();

    return GetSingle(user.getId());
  }

  @Override
  public User Create(User user) throws SQLException {
    Connection connection = connector.getConnection();

    String sql = "insert into \"EmotionsTrackingWebsite\".users (username, password, email)  values (?,?,?) returning *;";
    PreparedStatement statement = connection.prepareStatement(sql);
    statement.setString(1, user.getUsername());
    statement.setString(2, user.getPassword());
    statement.setString(3, user.getEmail());

    ResultSet resultSet = statement.executeQuery();
    while (resultSet.next()) {
      user.setId(resultSet.getInt("id"));
      user.setUsername(resultSet.getString("username"));
      user.setPassword(resultSet.getString("password"));
      user.setEmail(resultSet.getString("email"));
      user.setCreatedAt(resultSet.getTimestamp("created_at"));
      user.setUpdatedAt(resultSet.getTimestamp("updated_at"));
    }

    return user;
  }

  private static User parseUserFromDB(ResultSet resultSet) throws SQLException {
    return new User(
        resultSet.getInt("id"),
        resultSet.getString("username"),
        resultSet.getString("password"),
        resultSet.getString("email"),
        resultSet.getTimestamp("created_at"),
        resultSet.getTimestamp("updated_at"),
        resultSet.getInt("current_streak")
    );
  }
}
