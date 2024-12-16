package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public interface EmotionCheckInsDAO {

  EmotionCheckIn GetSingle(int id) throws SQLException;

  EmotionCheckIn Create(EmotionCheckIn emotionCheckIn, List<Tag> tags) throws SQLException;

  void Delete(int id) throws SQLException;

  EmotionCheckIn Update(EmotionCheckIn emotion, List<Tag> tags) throws SQLException;

  ArrayList<EmotionCheckIn> GetAll(int userId) throws SQLException;
  List<EmotionCheckIn> GetByTag(int userId, HashMap<String, String> tags) throws SQLException;
}
