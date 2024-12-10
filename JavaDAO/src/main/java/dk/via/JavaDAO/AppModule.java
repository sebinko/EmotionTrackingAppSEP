package dk.via.JavaDAO;

import com.google.inject.AbstractModule;
import dk.via.JavaDAO.DAO.EmotionCheckInsDAO;
import dk.via.JavaDAO.DAO.EmotionCheckInsDAODB;
import dk.via.JavaDAO.DAO.TagsDAO;
import dk.via.JavaDAO.DAO.TagsDAODB;
import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.DAO.UserFriendsDAODB;
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
    bind(UsersDAO.class).to(UsersDAODB.class);
    bind(TagsDAO.class).to(TagsDAODB.class);
    bind(EmotionCheckInsDAO.class).to(EmotionCheckInsDAODB.class);
    bind(UserFriendsDAO.class).to(UserFriendsDAODB.class);
  }
}