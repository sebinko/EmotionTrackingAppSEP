package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.User;

public interface UserFriendsDAO {

  public void CreateFriendship(Integer user1Id, Integer user2Id);

}
