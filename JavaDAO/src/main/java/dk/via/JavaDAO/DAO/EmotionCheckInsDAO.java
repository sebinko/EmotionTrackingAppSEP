package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import java.util.ArrayList;

public interface EmotionCheckInsDAO {

  EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, ArrayList<String> tags);

}
