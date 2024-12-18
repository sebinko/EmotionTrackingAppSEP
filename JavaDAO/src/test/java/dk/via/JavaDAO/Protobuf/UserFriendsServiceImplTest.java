package dk.via.JavaDAO.Protobuf;

import dk.via.JavaDAO.DAO.UserFriendsDAO;
import dk.via.JavaDAO.Models.Friendship;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Protobuf.Users.FriendsWithCheckIns;
import dk.via.JavaDAO.Protobuf.Users.FriendshipMessage;
import dk.via.JavaDAO.Protobuf.Users.FriendshipSimpleMessage;
import dk.via.JavaDAO.Protobuf.Users.UserFriendsServiceImpl;
import dk.via.JavaDAO.Protobuf.Users.UserId;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;

import java.sql.SQLException;
import java.util.HashMap;
import java.util.List;

import static org.mockito.Mockito.*;

public class UserFriendsServiceImplTest {

  private UserFriendsServiceImpl userFriendsService;

  @Mock
  private UserFriendsDAO userFriendsDAO;

  @Mock
  private StreamObserver<FriendshipMessage> responseObserver;

  @Mock
  private StreamObserver<FriendsWithCheckIns> responseObserverFriends;

  @BeforeEach
  public void setUp() {
    MockitoAnnotations.openMocks(this);
    userFriendsService = new UserFriendsServiceImpl(userFriendsDAO);
  }

  @Test
  public void testCreateFriendship_Success() throws SQLException {
    // Arrange
    FriendshipSimpleMessage request = FriendshipSimpleMessage.newBuilder()
        .setUser1Id(1)
        .setUser2Id(2)
        .build();  // manually constructing the protobuf message

    // Act
    userFriendsService.createFriendship(request, responseObserver);

    // Assert
    verify(userFriendsDAO, times(1)).CreateFriendship(1, 2);
    verify(responseObserver, times(1)).onNext(any());
    verify(responseObserver, times(1)).onCompleted();
  }

  @Test
  public void testRemoveFriendship_Success() throws SQLException {
    // Arrange
    FriendshipSimpleMessage request = FriendshipSimpleMessage.newBuilder()
        .setUser1Id(1)
        .setUser2Id(2)
        .build();  // manually constructing the protobuf message

    // Act
    userFriendsService.removeFriendship(request, responseObserver);

    // Assert
    verify(userFriendsDAO, times(1)).RemoveFriendship(1, 2);
    verify(responseObserver, times(1)).onNext(any());
    verify(responseObserver, times(1)).onCompleted();
  }


}

