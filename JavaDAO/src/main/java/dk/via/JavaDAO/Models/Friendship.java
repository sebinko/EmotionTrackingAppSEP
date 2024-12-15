package dk.via.JavaDAO.Models;

public class Friendship {
  private final Integer user1Id;
  private final Integer user2Id;
  private boolean isAccepted;

  public Friendship(Integer user1Id, Integer user2Id) {
    this.user1Id = user1Id;
    this.user2Id = user2Id;
  }

  public Friendship(Integer user1Id, Integer user2Id, boolean isAccepted) {
    this.user1Id = user1Id;
    this.user2Id = user2Id;
    this.isAccepted = isAccepted;
  }

  public Integer getUser1Id() {
    return user1Id;
  }

  public Integer getUser2Id() {
    return user2Id;
  }

  public boolean isAccepted() {
    return isAccepted;
  }

  public void setAccepted(boolean accepted) {
    isAccepted = accepted;
  }
}
