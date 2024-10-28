package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;

public class UsersDAODB implements UsersDAO {

  private DBConnector connector;

  @Inject
  public UsersDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public ArrayList<User> GetAll() {
    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".users;";
    ArrayList<User> users = new ArrayList<>();
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        int columns = resultSet.getMetaData().getColumnCount();
        User user = new User(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            resultSet.getObject(5).toString(),
            resultSet.getObject(6).toString()
        );
        users.add(user);
      }
    } catch (Exception e) {
      System.out.println(e.getMessage());
    }
    return users;
  }

  @Override
  public User GetSingle(int id) {

    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".users where id = ?;";
    User user = null;

    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        user = new User(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            resultSet.getObject(5).toString(),
            resultSet.getObject(6).toString()
        );

      }
    } catch (Exception e) {
      System.out.println(e.getMessage());
    }
    return user;
  }

  @Override
  public User Update(User user) {
    Connection connection = connector.getConnection();

    User userToUpdate = GetSingle(user.getId());
    if (userToUpdate == null) {
      throw new RuntimeException("User not found");
    }
    String sql = "update \"EmotionsTrackingWebsite\".users set username=?, password= ?, email= ?, updated_at= now() where id=?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, user.getUsername());
      statement.setString(2, user.getPassword());
      statement.setString(3, user.getEmail());
      statement.setInt(4, user.getId());
      statement.executeUpdate();
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return user;
  }

  @Override
  public User Create(User user) {
    Connection connection = connector.getConnection();

    String sql = "insert into \"EmotionsTrackingWebsite\".users (username, password, email)  values (?,?,?) returning id;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setString(1, user.getUsername());
      statement.setString(2, user.getPassword());
      statement.setString(3, user.getEmail());

      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        user.setId(Integer.parseInt(resultSet.getObject(1).toString()));
      }
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return user;
  }

  @Override
  public User Delete(User user) {
    Connection connection = connector.getConnection();

    User userToUpdate = GetSingle(user.getId());
    if (userToUpdate == null) {
      throw new RuntimeException("User not found");
    }
    String sql = "delete from \"EmotionsTrackingWebsite\".users where id= ?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, user.getId());
      statement.executeUpdate();
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return user;
  }
}
