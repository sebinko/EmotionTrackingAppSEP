package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import java.util.ArrayList;

public interface EmotionCheckInsDAO {

  EmotionCheckIn GetSingle(int id);

  EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, ArrayList<String> tags);

  EmotionCheckIn Delete(EmotionCheckIn emotionCheckIn);

  EmotionCheckIn Update(EmotionCheckIn emotion, ArrayList<Tag> existingTags,
      ArrayList<String> newTags);
}
