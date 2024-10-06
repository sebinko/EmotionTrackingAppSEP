package dk.via.JavaDAO;

import com.google.inject.Guice;
import com.google.inject.Injector;
import dk.via.JavaDAO.Status.StatusServiceImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;

/**
 * Main class to start the server.
 */
public class Main {

  /**
   * Main method to start the server.
   *
   * @param args command line arguments
   */
  public static void main(String[] args) {
    Injector injector = Guice.createInjector(new AppModule());

    Server server = ServerBuilder.forPort(8888)
        .addService(injector.getInstance(StatusServiceImpl.class))
        .build();

    try {
      server.start();
      server.awaitTermination();
    } catch (Exception e) {
      e.printStackTrace();
    }
  }
}
