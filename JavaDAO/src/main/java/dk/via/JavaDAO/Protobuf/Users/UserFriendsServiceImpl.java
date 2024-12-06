package dk.via.JavaDAO.Protobuf.Users;

import com.google.inject.Inject;
import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.Protobuf.Users.UserFriendsServiceGrpc.UserFriendsServiceImplBase;
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
    userFriendsDAO.CreateFriendship(request.getUser1Id(), request.getUser2Id());
    responseObserver.onNext(FriendshipMessage.newBuilder().build());
    responseObserver.onCompleted();
  }
}
