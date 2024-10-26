package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.User;
import java.util.ArrayList;

public interface UsersDAO {
  public ArrayList<User> GetAll();
  public User GetSingle(int id);
  public User Update( User user);
  public User Create (User user);
  public User Delete (User user);

}
