namespace dk.via.JavaDAO.Models
{
  public class EmotionCheckIn
  {
    public int? Id { get; set; }
    public string Emotion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UserId { get; set; }
  }
}