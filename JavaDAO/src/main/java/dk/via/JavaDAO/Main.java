package dk.via.JavaDAO;

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
    Server server = ServerBuilder.forPort(8888)
        .addService(new StatusServiceImpl())
        .build();

    try {
      server.start();
      server.awaitTermination();
    } catch (Exception e) {
      e.printStackTrace();
    }
  }
}
