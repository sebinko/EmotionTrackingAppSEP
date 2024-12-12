package dk.via.JavaDAO.Protobuf.Users;

import  com.google.inject.Inject;
import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.Protobuf.Users.UserFriendsServiceGrpc.UserFriendsServiceImplBase;
import dk.via.JavaDAO.Util.PSQLExceptionParser;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import org.postgresql.util.PSQLException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.sql.SQLException;

public class UserFriendsServiceImpl extends UserFriendsServiceImplBase {

  private final UserFriendsDAO userFriendsDAO;
  private final Logger logger = LoggerFactory.getLogger(UserFriendsServiceImpl.class.getName());

  @Inject
  public UserFriendsServiceImpl(UserFriendsDAO userFriendsDAO) {
    this.userFriendsDAO = userFriendsDAO;
  }

  @Override
  public void createFriendship(FriendshipMessage request,
      StreamObserver<FriendshipMessage> responseObserver) {
    try {
      userFriendsDAO.CreateFriendship(request.getUser1Id(), request.getUser2Id());

      responseObserver.onNext(FriendshipMessage
          .newBuilder()
          .setUser1Id(request.getUser1Id())
          .setUser2Id(request.getUser2Id())
          .build());

      responseObserver.onCompleted();
    } catch (PSQLException e) {
      PSQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }


  public void removeFriendship(FriendshipMessage request,
    StreamObserver<FriendshipMessage> responseObserver) {
    try {
      userFriendsDAO.RemoveFriendship(request.getUser1Id(), request.getUser2Id());

      responseObserver.onNext(FriendshipMessage
          .newBuilder()
          .setUser1Id(request.getUser1Id())
          .setUser2Id(request.getUser2Id())
          .build());

      responseObserver.onCompleted();
    } catch (SQLException e) {
      PSQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}
