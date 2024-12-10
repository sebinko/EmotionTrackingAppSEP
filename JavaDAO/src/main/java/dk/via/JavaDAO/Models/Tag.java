package dk.via.JavaDAO.Models;

import java.sql.Timestamp;
import java.util.Objects;

public class Tag {

  private Integer id;
  private String key;
  private TagType type;
  private Timestamp createdAt;
  private Timestamp updatedAt;
  private Integer userId;

  public Tag() {
    id = null;
    key = null;
    type = null;
    createdAt = null;
    updatedAt = null;
    userId = null;
  }

  public Tag(Integer id, String key, TagType type, Timestamp createdAt, Timestamp updatedAt,
      Integer userId) {
    this.id = id;
    this.key = key;
    this.type = type;
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

  public TagType getType() {
    return type;
  }

  public void setType(TagType type) {
    this.type = type;
  }

  public Timestamp getCreatedAt() {
    return createdAt;
  }

  public void setCreatedAt(Timestamp createdAt) {
    this.createdAt = createdAt;
  }

  public Timestamp getUpdatedAt() {
    return updatedAt;
  }

  public void setUpdatedAt(Timestamp updatedAt) {
    this.updatedAt = updatedAt;
  }

  public Integer getUserId() {
    return userId;
  }

  public void setUserId(Integer userId) {
    this.userId = userId;
  }

  @Override
  public boolean equals(Object obj) {
    if (this == obj) {
      return true;
    }
    if (obj == null || getClass() != obj.getClass()) {
      return false;
    }
    Tag tag = (Tag) obj;

    return
        key.equals(tag.key) &&
        type == tag.type &&
        userId.equals(tag.userId);
  }

  @Override
  public int hashCode() {
    return Objects.hash(key, type, userId);
  }
}