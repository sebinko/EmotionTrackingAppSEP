package dk.via.JavaDAO;

import com.google.inject.AbstractModule;
import dk.via.JavaDAO.DAO.UsersDAO;
import dk.via.JavaDAO.DAO.UsersDAODB;
import dk.via.JavaDAO.Services.EmotionListService;
import dk.via.JavaDAO.Util.AppConfig;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Util.PostgresConnector;

public class AppModule extends AbstractModule {

  @Override
  protected void configure() {
    bind(DBConnector.class).to(PostgresConnector.class);
    bind(AppConfig.class).asEagerSingleton();
    bind(EmotionListService.class).asEagerSingleton();
    bind(UsersDAO.class ).to(UsersDAODB.class);

  }
}
