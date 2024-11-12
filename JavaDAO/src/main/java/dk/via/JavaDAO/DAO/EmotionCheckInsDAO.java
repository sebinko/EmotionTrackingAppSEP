package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;

public interface EmotionCheckInsDAO {
  EmotionCheckIn GetSingle(int id);
  EmotionCheckIn Delete(EmotionCheckIn emotionCheckIn);
}
