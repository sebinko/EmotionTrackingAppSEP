package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Models.TagType;
import dk.via.JavaDAO.Models.User;
import java.sql.SQLException;
import java.util.List;

public interface TagsDAO {

  Tag GetSingle(int id) throws SQLException;
  Tag GetSingle(String key, TagType type, Integer userId) throws SQLException;

  List<Tag> GetAllForCheckIn(EmotionCheckIn emotionCheckIn) throws SQLException;
  List<Tag> GetAllForUser(int userId) throws SQLException;

  void AssignTag(Tag tag, EmotionCheckIn checkIn) throws SQLException;

  void RemoveTag(Tag tag, EmotionCheckIn checkIn) throws SQLException;
}
