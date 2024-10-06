package dk.via.JavaDAO.Status;

import dk.via.JavaDAO.Status.StatusServiceGrpc.StatusServiceImplBase;

/**
 * StatusServiceImpl class to implement the getStatusMethod.
 */
public class StatusServiceImpl extends StatusServiceImplBase {

  /**
   * Method to get the status of the server.
   *
   * @param request StatusRequest
   * @param responseObserver StreamObserver
   */
  @Override
  public void getStatusMethod(dk.via.JavaDAO.Status.StatusRequest request,
      io.grpc.stub.StreamObserver<dk.via.JavaDAO.Status.StatusMessage> responseObserver) {

    StatusMessage reply = StatusMessage.newBuilder().setStatus("NOT OK")
        .setMessage("CLIENT RUNNING, BUT DATABASE NOT CONNECTED").build();

    responseObserver.onNext(reply);
    responseObserver.onCompleted();
  }
}
