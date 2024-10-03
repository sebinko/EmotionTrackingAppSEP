package dk.via.JavaDAO.Status;

import dk.via.JavaDAO.Status.StatusServiceGrpc.StatusServiceImplBase;

public class StatusServiceImpl extends StatusServiceImplBase {
    @Override
    public void getStatusMethod(dk.via.JavaDAO.Status.StatusRequest request,
                                io.grpc.stub.StreamObserver<dk.via.JavaDAO.Status.StatusMessage> responseObserver) {
        StatusMessage reply = StatusMessage.newBuilder().setStatus("OK").setMessage("Status is OK").build();
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }
}
