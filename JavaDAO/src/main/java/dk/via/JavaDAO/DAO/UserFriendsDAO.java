package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Friendship;
import dk.via.JavaDAO.Models.User;
import java.sql.SQLException;

public interface UserFriendsDAO {
  public void CreateFriendship(Integer user1Id, Integer user2Id) throws SQLException;
  public Friendship GetFriendShip(Integer user1Id, Integer user2Id) throws SQLException;
  public Friendship UpdateFriendship(Friendship friendship) throws SQLException;
  public void RemoveFriendship(Integer user1Id, Integer user2Id) throws SQLException;
}
