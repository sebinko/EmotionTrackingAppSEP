package dk.via.JavaDAO.Models;

import java.util.Objects;

public class Emotion {

  private String emotion;
  private Color color;
  private String description;

  public Emotion(String emotion, Color color, String description) {
    this.emotion = emotion;
    this.color = color;
    this.description = description;
  }

  public String getEmotion() {
    return emotion;
  }

  public void setEmotion(String emotion) {
    this.emotion = emotion;
  }

  public Color getColor() {
    return color;
  }

  public void setColor(Color color) {
    this.color = color;
  }

  public String getDescription() {
    return description;
  }

  public void setDescription(String description) {
    this.description = description;
  }

  @Override
  public boolean equals(Object o) {
    if (this == o)
      return true;
    if (o == null || getClass() != o.getClass())
      return false;
    Emotion emotion1 = (Emotion) o;
    return Objects.equals(emotion, emotion1.emotion) && color == emotion1.color && Objects.equals(
        description, emotion1.description);
  }

  @Override
  public int hashCode() {
    return Objects.hash(emotion, color, description);
  }
}

