package dk.via.JavaDAO.Models;

public class Tag {
  private Integer id;
  private String key;
  private String createdAt;
  private String updatedAt;
  private Integer userId;

  public Tag() {
    id=null;
    key= null;
    createdAt= null;
    updatedAt=null;
    userId=null;
  }

  public Tag(Integer id, String key, String createdAt, String updatedAt,
      Integer userId) {
    this.id = id;
    this.key = key;
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

  public String getKey() {
    return key;
  }

  public void setKey(String key) {
    this.key = key;
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