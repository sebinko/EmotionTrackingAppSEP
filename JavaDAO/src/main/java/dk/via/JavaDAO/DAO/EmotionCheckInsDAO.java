package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import java.util.ArrayList;

public interface EmotionCheckInsDAO {

  EmotionCheckIn GetSingle(int id);

  EmotionCheckIn Delete(EmotionCheckIn emotionCheckIn);

  EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, ArrayList<String> tags);
}
