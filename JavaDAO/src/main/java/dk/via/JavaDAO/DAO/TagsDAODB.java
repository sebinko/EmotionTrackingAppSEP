package dk.via.JavaDAO.DAO;

import com.google.inject.Inject;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Tag;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;

public class TagsDAODB implements TagsDAO {

  private final DBConnector connector;
  private final Logger logger = LoggerFactory.getLogger(TagsDAODB.class.getName());

  @Inject
  public TagsDAODB(DBConnector connector) {
    super();
    this.connector = connector;
  }

  @Override
  public Tag GetSingle(int id) {

    Connection connection = connector.getConnection();
    String sql = "select * from \"EmotionsTrackingWebsite\".tags where id = ?;";
    Tag tag = null;

    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, id);
      ResultSet resultSet = statement.executeQuery();
      while (resultSet.next()) {
        tag = new Tag(
            Integer.parseInt(resultSet.getObject(1).toString()),
            resultSet.getObject(2).toString(),
            resultSet.getObject(3).toString(),
            resultSet.getObject(4).toString(),
            Integer.parseInt(resultSet.getObject(5).toString())
        );

      }
    } catch (Exception e) {
      logger.error(e.getMessage());
    }
    return tag;
  }

  @Override
  public Tag Delete(Tag tag) {

    Connection connection = connector.getConnection();

    Tag tagToUpdate = GetSingle(tag.getId());
    if (tagToUpdate == null) {
      throw new RuntimeException("Tag not found");
    }
    String sql = "delete from \"EmotionsTrackingWebsite\".tags where id= ?;";
    try {
      PreparedStatement statement = connection.prepareStatement(sql);
      statement.setInt(1, tag.getId());
      statement.executeUpdate();
    } catch (Exception e) {
      throw new RuntimeException(e);
    }
    return tag;
  }

}
