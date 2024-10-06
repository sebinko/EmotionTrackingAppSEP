package dk.via.JavaDAO.Util.Interfaces;

import java.sql.Connection;

public interface DBConnector {

  void executeUpdate(String sql, Object... params);

  Connection getConnection();
}
