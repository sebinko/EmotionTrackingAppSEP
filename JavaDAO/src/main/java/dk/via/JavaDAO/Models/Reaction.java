package dk.via.JavaDAO.Models;

import java.sql.Timestamp;

public class Reaction {

  private Integer userId;
  private Integer emotionCheckinId;
  private String emoji;
  private Timestamp createdAt;
  private Timestamp updatedAt;

  public Reaction() {
    userId = null;
    emotionCheckinId = null;
    emoji = null;
    createdAt = null;
    updatedAt = null;
  }

  public Reaction(Integer userId, Integer emotionCheckinId, String emoji, Timestamp createdAt, Timestamp updatedAt) {
    this.userId = userId;
    this.emotionCheckinId = emotionCheckinId;
    this.emoji = emoji;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
  }

  public Integer getUserId() {
    return userId;
  }

  public void setUserId(Integer userId) {
    this.userId = userId;
  }

  public Integer getEmotionCheckinId() {
    return emotionCheckinId;
  }

  public void setEmotionCheckinId(Integer emotionCheckinId) {
    this.emotionCheckinId = emotionCheckinId;
  }

  public String getEmoji() {
    return emoji;
  }

  public void setEmoji(String emoji) {
    this.emoji = emoji;
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

}
