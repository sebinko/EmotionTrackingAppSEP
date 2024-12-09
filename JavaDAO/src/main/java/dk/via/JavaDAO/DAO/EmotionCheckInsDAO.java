package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import java.util.ArrayList;

public interface EmotionCheckInsDAO {

  EmotionCheckIn GetSingle(int id);

  EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, ArrayList<String> tags);

  EmotionCheckIn Delete(int id);

  EmotionCheckIn Update(EmotionCheckIn emotion, ArrayList<Tag> existingTags,
      ArrayList<String> newTags);

  ArrayList<EmotionCheckIn> GetAll(int userId);
}
