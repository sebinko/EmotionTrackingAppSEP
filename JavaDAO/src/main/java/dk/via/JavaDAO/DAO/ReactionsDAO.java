package dk.via.JavaDAO.DAO;
import dk.via.JavaDAO.Models.Reaction;
import java.sql.SQLException;
import java.util.List;

public interface ReactionsDAO {
  Reaction Create (Reaction reaction) throws SQLException;
  void Delete (Integer userId, Integer emotionCheckinId) throws SQLException;
  List<Reaction> GetReactionsForEmotionCheckIn(Integer emotionCheckInId) throws SQLException;
}
