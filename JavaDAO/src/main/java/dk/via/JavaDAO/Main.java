package dk.via.JavaDAO;

import com.google.inject.Guice;
import com.google.inject.Injector;
import dk.via.JavaDAO.Protobuf.Emotions.EmotionsServiceImpl;
import dk.via.JavaDAO.Services.EmotionListService;
import dk.via.JavaDAO.Status.StatusServiceImpl;
import dk.via.JavaDAO.Util.AppConfig;
import io.grpc.Server;
import io.grpc.ServerBuilder;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Main class to start the server.
 */
public class Main {
  private static final Logger logger = LoggerFactory.getLogger(Main.class.getName());

  /**
   * Main method to start the server.
   *
   * @param args command line arguments
   */
  public static void main(String[] args) {
    Injector injector = Guice.createInjector(new AppModule());

    Server server = ServerBuilder.forPort(8888)
        .addService(injector.getInstance(StatusServiceImpl.class))
        .addService(injector.getInstance(EmotionsServiceImpl.class))
        .build();

    try {
      server.start();
      server.awaitTermination();
    } catch (Exception e) {
      logger.error("Error starting server: {}", e.getMessage());
    }
  }
}
