package dk.via.JavaDAO.Protobuf;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

import dk.via.JavaDAO.Protobuf.Reactions.ReactionServiceGrpc;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionCreateMessage;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionDeleteMessage;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionMessage;
import dk.via.JavaDAO.DAO.ReactionsDAO;
import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionsServiceImpl;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;
import org.postgresql.util.PSQLException;

import java.sql.SQLException;
import java.sql.Timestamp;

public class ReactionsServiceImplTest {

  @Mock
  private ReactionsDAO reactionsDAO;

  @Mock
  private StreamObserver<ReactionMessage> responseObserver;

  private ReactionsServiceImpl reactionsService;

  @BeforeEach
  public void setUp() {
    MockitoAnnotations.openMocks(this); // Initialize mocks
    reactionsService = new ReactionsServiceImpl(reactionsDAO);
  }

  @Test
  public void testCreateReaction() throws SQLException {
    // Setup
    ReactionCreateMessage request = ReactionCreateMessage.newBuilder()
        .setEmoji("😊")
        .setUserId(1)
        .setEmotionCheckInId(100)
        .build();

    Reaction mockedReaction = new Reaction(1, 100, "😊", new Timestamp(System.currentTimeMillis()), new Timestamp(System.currentTimeMillis()));
    when(reactionsDAO.Create(any(Reaction.class))).thenReturn(mockedReaction);

    // Call create
    reactionsService.create(request, responseObserver);

    // Verify interactions
    verify(reactionsDAO).Create(any(Reaction.class));
    verify(responseObserver).onNext(any(ReactionMessage.class));
    verify(responseObserver).onCompleted();
  }

  @Test
  public void testCreateReactionSQLException() throws SQLException {
    // Setup
    ReactionCreateMessage request = ReactionCreateMessage.newBuilder()
        .setEmoji("😊")
        .setUserId(1)
        .setEmotionCheckInId(100)
        .build();

    when(reactionsDAO.Create(any(Reaction.class))).thenThrow(new PSQLException("SQL Error", null));

    // Call create and expect an error
    reactionsService.create(request, responseObserver);

    // Verify error handling
    verify(responseObserver).onError(argThat(e -> e instanceof io.grpc.StatusException));
  }

  @Test
  public void testDeleteReaction() throws SQLException {
    // Setup
    ReactionDeleteMessage request = ReactionDeleteMessage.newBuilder()
        .setUserId(1)
        .setEmotionCheckInId(100)
        .build();

    // Call delete
    reactionsService.delete(request, responseObserver);

    // Verify interactions
    verify(reactionsDAO).Delete(1, 100);
    verify(responseObserver).onNext(any(ReactionMessage.class));
    verify(responseObserver).onCompleted();
  }

  @Test
  public void testDeleteReactionSQLException() throws SQLException {
    // Setup
    ReactionDeleteMessage request = ReactionDeleteMessage.newBuilder()
        .setUserId(1)
        .setEmotionCheckInId(100)
        .build();

    doThrow(new PSQLException("SQL Error", null)).when(reactionsDAO).Delete(1, 100);

    // Call delete and expect an error
    reactionsService.delete(request, responseObserver);

    // Verify error handling
    verify(responseObserver).onError(argThat(e -> e instanceof io.grpc.StatusException));
  }
}
