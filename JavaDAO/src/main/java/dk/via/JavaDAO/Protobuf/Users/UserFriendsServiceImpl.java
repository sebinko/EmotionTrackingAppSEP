package dk.via.JavaDAO.Protobuf.Users;

import com.google.inject.Inject;
import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.Protobuf.Users.UserFriendsServiceGrpc.UserFriendsServiceImplBase;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;

public class UserFriendsServiceImpl extends UserFriendsServiceImplBase {

  private final UserFriendsDAO userFriendsDAO;

  @Inject
  public UserFriendsServiceImpl(UserFriendsDAO userFriendsDAO) {
    this.userFriendsDAO = userFriendsDAO;
  }

  @Override
  public void createFriendship(FriendshipMessage request,
      StreamObserver<FriendshipMessage> responseObserver) {
    try {
      userFriendsDAO.CreateFriendship(request.getUser1Id(), request.getUser2Id());
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withDescription(e.getMessage()).asRuntimeException());
    }

    responseObserver.onNext(FriendshipMessage
        .newBuilder()
        .setUser1Id(request.getUser1Id())
        .setUser2Id(request.getUser2Id())
        .build());

    responseObserver.onCompleted();
  }
}
