package dk.via.JavaDAO.Models;

import java.sql.Timestamp;

public class User {

  private Integer id;
  private String username;
  private String password;
  private String email;
  private Timestamp createdAt;
  private Timestamp updatedAt;
  private Integer streak;

  public User(Integer id, String username, String password, String email,
      Timestamp createdAt, Timestamp updatedAt, Integer streak) {
    this.id = id;
    this.username = username;
    this.password = password;
    this.email = email;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
    this.streak = streak;
  }

  public User(String username, String password, String email, Timestamp createdAt,
      Timestamp updatedAt, Integer streak) {
    this.username = username;
    this.password = password;
    this.email = email;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
    this.streak = streak;
  }

  public Integer getId() {
    return id;
  }

  public void setId(Integer id) {
    this.id = id;
  }

  public String getUsername() {
    return username;
  }

  public void setUsername(String username) {
    this.username = username;
  }

  public String getPassword() {
    return password;
  }

  public void setPassword(String password) {
    this.password = password;
  }

  public String getEmail() {
    return email;
  }

  public void setEmail(String email) {
    this.email = email;
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

  public int getStreak() {
    return streak;
  }

  public void setStreak(Integer streak) {
    this.streak = streak;
  }
}
