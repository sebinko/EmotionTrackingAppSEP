package dk.via.JavaDAO.Protobuf.Users;

import com.google.inject.Inject;
import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.Friendship;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Protobuf.Users.UserFriendsServiceGrpc.UserFriendsServiceImplBase;
import dk.via.JavaDAO.Util.SQLExceptionParser;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.List;
import org.postgresql.util.PSQLException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UserFriendsServiceImpl extends UserFriendsServiceImplBase {

  private final UserFriendsDAO userFriendsDAO;
  private final Logger logger = LoggerFactory.getLogger(UserFriendsServiceImpl.class.getName());

  @Inject
  public UserFriendsServiceImpl(UserFriendsDAO userFriendsDAO) {
    this.userFriendsDAO = userFriendsDAO;
  }

  @Override
  public void createFriendship(FriendshipSimpleMessage request,
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
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void removeFriendship(FriendshipSimpleMessage request,
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
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getFriendship(FriendshipSimpleMessage request,
      StreamObserver<FriendshipMessage> responseObserver) {
    try {
      Friendship friendship = userFriendsDAO.GetFriendShip(request.getUser1Id(),
          request.getUser2Id());

      if (friendship == null) {
        responseObserver.onNext(FriendshipMessage
            .newBuilder()
            .build());
        responseObserver.onCompleted();
        return;
      }

      responseObserver.onNext(FriendshipMessage
          .newBuilder()
          .setUser1Id(friendship.getUser1Id())
          .setUser2Id(friendship.getUser2Id())
          .setIsAccepted(friendship.isAccepted())
          .build());
      responseObserver.onCompleted();

    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }

  }

  @Override
  public void updateFriendShip(FriendshipMessage request,
      StreamObserver<FriendshipMessage> responseObserver) {
    try {
      Friendship friendship = new Friendship(request.getUser1Id(), request.getUser2Id(),
          request.getIsAccepted());
      userFriendsDAO.UpdateFriendship(friendship);

      responseObserver.onNext(FriendshipMessage
          .newBuilder()
          .setUser1Id(request.getUser1Id())
          .setUser2Id(request.getUser2Id())
          .setIsAccepted(request.getIsAccepted())
          .build());

      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getAllFriends(UserId request, StreamObserver<FriendsWithCheckIns> responseObserver) {
    try {
      HashMap<User, EmotionCheckIn> friendsWithCheckIn = userFriendsDAO.GetFriendsWithCheckIn(request.getId());

      FriendsWithCheckIns.Builder friendsWithCheckInsBuilder = FriendsWithCheckIns.newBuilder();

      for (User user : friendsWithCheckIn.keySet()) {
        EmotionCheckIn emotionCheckIn = friendsWithCheckIn.get(user);

        friendsWithCheckInsBuilder.addFriends(FriendWithCheckIn.newBuilder()
            .setFriend(dk.via.JavaDAO.Protobuf.Users.User.newBuilder()
                .setId(user.getId())
                .setUsername(user.getUsername())
                .setEmail(user.getEmail())
                .setCreatedAt(user.getCreatedAt().toString())
                .setUpdatedAt(user.getUpdatedAt().toString())
                .setStreak(user.getStreak())
                .build())
            .setEmotion(dk.via.JavaDAO.Protobuf.Users.SimpleEmotionCheckIn.newBuilder()
                .setEmotion(emotionCheckIn.getEmotion())
                .setUpdatedAt(emotionCheckIn.getUpdatedAt().toString())
                .setId(emotionCheckIn.getId()))
                .build()
            )
            .build();
      }

      responseObserver.onNext(friendsWithCheckInsBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}