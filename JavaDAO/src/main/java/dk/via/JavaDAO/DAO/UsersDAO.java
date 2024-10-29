package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.User;
import java.util.ArrayList;

public interface UsersDAO {

  ArrayList<User> GetAll();

  User GetSingle(int id);

  User Update(User user);

  User Create(User user);

  User Delete(User user);

}
