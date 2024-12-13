package dk.via.JavaDAO.DAO;
import dk.via.JavaDAO.Models.Reaction;
import java.sql.SQLException;

public interface ReactionsDAO {
  Reaction Create (Reaction reaction) throws SQLException;
}
