package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Util.AppConfig;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Util.PostgresConnector;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class ReactionsDAODBIntegrationTests {
  private ReactionsDAO reactionsDAO;
  private DBConnector dbConnector;
  private Connection connection;

  @BeforeEach
  void setUp() throws SQLException {
    dbConnector = new PostgresConnector(new AppConfig());
    connection = dbConnector.getConnection();
    reactionsDAO = new ReactionsDAODB(dbConnector);
  }

  private void deleteReactionIfExists(int reactionId, int userId) throws SQLException {
    String sql = "DELETE FROM \"EmotionsTrackingWebsite\".reactions WHERE emotion_checkin_id = ? AND user_id = ?";
    try (PreparedStatement statement = connection.prepareStatement(sql)) {
      statement.setInt(1, reactionId);
      statement.setInt(2, userId);
      statement.executeUpdate();
    }
  }

  @Test
  void testCreateReaction() throws SQLException {
    deleteReactionIfExists(1, 1);

    Reaction reaction = new Reaction(1, 1, "Like", null, null);
    Reaction createdReaction = reactionsDAO.Create(reaction);

    assertNotNull(createdReaction);
    assertEquals("Like", createdReaction.getEmoji());
    assertEquals(1, createdReaction.getUserId());
    assertEquals(1, createdReaction.getEmotionCheckinId());
  }

  @Test
  void testGetAllReactions() throws SQLException {
    List<Reaction> reactions = reactionsDAO.GetReactionsForEmotionCheckIn(1);

    assertNotNull(reactions);
    /*assertEquals(3, reactions.size());*/ //this may break the test if there are more reactions added from other tests
  }

  @Test
  void testDeleteReaction() throws SQLException {
    Reaction reaction = new Reaction(2, 2, "Like", null, null);
    reactionsDAO.Create(reaction);
    List<Reaction> reactionsBeforeDelete = reactionsDAO.GetReactionsForEmotionCheckIn(1);
    int initialCount = reactionsBeforeDelete.size();

    reactionsDAO.Delete(2, 2);

    List<Reaction> reactionsAfterDelete = reactionsDAO.GetReactionsForEmotionCheckIn(1);
    int finalCount = reactionsAfterDelete.size();
    assertEquals(initialCount, finalCount);
  }

  @Test
  void testDeleteNonExistentReaction() throws SQLException {
    List<Reaction> reactionsBeforeDelete = reactionsDAO.GetReactionsForEmotionCheckIn(1);
    int initialCount = reactionsBeforeDelete.size();

    reactionsDAO.Delete(999, 999);

    List<Reaction> reactionsAfterDelete = reactionsDAO.GetReactionsForEmotionCheckIn(1);
    int finalCount = reactionsAfterDelete.size();

    assertEquals(initialCount, finalCount);
  }

  @Test
  void testCreateReactionWithInvalidData() {
    Reaction invalidReaction = new Reaction(null, null, null, null, null);
    assertThrows(NullPointerException.class, () -> {
      reactionsDAO.Create(invalidReaction);
    });
  }

  @Test
  void testGetReactionsForNonExistentEmotionCheckIn() throws SQLException {
    List<Reaction> reactions = reactionsDAO.GetReactionsForEmotionCheckIn(999);
    assertTrue(reactions.isEmpty());
  }
}
