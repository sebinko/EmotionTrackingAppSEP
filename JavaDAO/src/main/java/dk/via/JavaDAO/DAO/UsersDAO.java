package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.User;
import java.sql.SQLException;
import java.util.ArrayList;

public interface UsersDAO {

  ArrayList<User> GetAll() throws SQLException;

  User GetSingle(int id) throws SQLException;
  User GetSingle(String username, String password) throws SQLException;

  User Update(User user) throws SQLException;

  User Create(User user) throws SQLException;

}
