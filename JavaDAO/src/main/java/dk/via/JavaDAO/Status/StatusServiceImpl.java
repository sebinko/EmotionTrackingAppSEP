package dk.via.JavaDAO.Status;

import com.google.inject.Inject;
import dk.via.JavaDAO.Status.StatusServiceGrpc.StatusServiceImplBase;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import java.sql.Connection;

/**
 * StatusServiceImpl class to implement the getStatusMethod.
 */
public class StatusServiceImpl extends StatusServiceImplBase {

  private DBConnector dbConnector;

  @Inject
  public StatusServiceImpl(DBConnector dbConnector) {
    super();

    this.dbConnector = dbConnector;
  }

  /**
   * Method to get the status of the database.
   *
   * @param request StatusRequest
   * @param responseObserver StreamObserver
   */
  @Override
  public void getStatusMethod(dk.via.JavaDAO.Status.StatusRequest request,
      io.grpc.stub.StreamObserver<dk.via.JavaDAO.Status.StatusMessage> responseObserver) {

    Connection connection = dbConnector.getConnection();

    if (connection != null) {
      StatusMessage reply = StatusMessage.newBuilder().setStatus("OK")
          .setMessage("CLIENT RUNNING, DATABASE CONNECTED").build();

      responseObserver.onNext(reply);
      responseObserver.onCompleted();
      return;
    }

    StatusMessage reply = StatusMessage.newBuilder().setStatus("NOT OK")
        .setMessage("CLIENT RUNNING, BUT DATABASE NOT CONNECTED").build();

    responseObserver.onNext(reply);
    responseObserver.onCompleted();
  }
}
