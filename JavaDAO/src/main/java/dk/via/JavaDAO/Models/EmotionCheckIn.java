package dk.via.JavaDAO.Models;

public class EmotionCheckIn {

  private Integer id;
  private String emotion;
  private String description;
  private String createdAt;
  private String updatedAt;
  private Integer userId;

  public EmotionCheckIn() {
    id = null;
    emotion = null;
    description = null;
    createdAt = null;
    updatedAt = null;
    userId = null;
  }

  public EmotionCheckIn(Integer id, String emotion, String description, String createdAt,
      String updatedAt, Integer userId) {
    this.id = id;
    this.emotion = emotion;
    this.description = description;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
    this.userId = userId;
  }

  public Integer getId() {
    return id;
  }

  public void setId(Integer id) {
    this.id = id;
  }

  public String getEmotion() {
    return emotion;
  }

  public void setEmotion(String emotion) {
    this.emotion = emotion;
  }

  public String getDescription(){
    return description;
  }

  public void setDescription(String description){
    this.description = description;
  }

  public String getCreatedAt() {
    return createdAt;
  }

  public void setCreatedAt(String createdAt) {
    this.createdAt = createdAt;
  }

  public String getUpdatedAt() {
    return updatedAt;
  }

  public void setUpdatedAt(String updatedAt) {
    this.updatedAt = updatedAt;
  }

  public Integer getUserId() {
    return userId;
  }

  public void setUserId(Integer userId) {
    this.userId = userId;
  }
}
