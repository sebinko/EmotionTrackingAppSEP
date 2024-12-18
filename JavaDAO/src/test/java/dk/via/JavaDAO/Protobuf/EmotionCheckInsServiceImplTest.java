package dk.via.JavaDAO.Protobuf;
import static org.mockito.Mockito.*;
import dk.via.JavaDAO.Protobuf.EmotionCheckIns.*;
import dk.via.JavaDAO.Models.*;
import dk.via.JavaDAO.DAO.EmotionCheckInsDAO;
import dk.via.JavaDAO.DAO.TagsDAO;
import dk.via.JavaDAO.Util.SQLExceptionParser;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;

import java.sql.SQLException;
import java.util.ArrayList;

import static org.mockito.Mockito.*;

import static org.junit.jupiter.api.Assertions.*;

public class EmotionCheckInsServiceImplTest {

  @Mock
  private EmotionCheckInsDAO emotionCheckInsDAO;

  @Mock
  private TagsDAO tagsDAO;

  @Mock
  private StreamObserver<EmotionCheckInMessage> responseObserver;

  private EmotionCheckInsServiceImpl emotionCheckInsServiceImpl;

  @BeforeEach
  public void setUp() {
    MockitoAnnotations.initMocks(this);
    emotionCheckInsServiceImpl = new EmotionCheckInsServiceImpl(emotionCheckInsDAO, tagsDAO);
  }


  @Test
  public void testGetById_Success() throws SQLException {
    // Arrange
    EmotionCheckInIdMessage request = EmotionCheckInIdMessage.newBuilder().setId(1).build();
    EmotionCheckIn mockEmotionCheckIn = new EmotionCheckIn();
    mockEmotionCheckIn.setId(1);
    mockEmotionCheckIn.setEmotion("Happy");
    mockEmotionCheckIn.setDescription("Feeling good");
    mockEmotionCheckIn.setUserId(123);

    // Mock the DAO to return the mocked EmotionCheckIn
    when(emotionCheckInsDAO.GetSingle(1)).thenReturn(mockEmotionCheckIn);

    // Act
    emotionCheckInsServiceImpl.getById(request, responseObserver);

    // Assert
    verify(responseObserver).onNext(argThat(emotionCheckInMessage ->
        emotionCheckInMessage.getId() == 1 &&
            emotionCheckInMessage.getEmotion().equals("Happy") &&
            emotionCheckInMessage.getDescription().equals("Feeling good") &&
            emotionCheckInMessage.getUserId() == 123
    ));
    verify(responseObserver).onCompleted();
  }





  @Test
  public void testGetById_NotFound() throws SQLException {
    // Prepare mock data
    when(emotionCheckInsDAO.GetSingle(1)).thenReturn(null); // Simulate a not found response

    // Call method
    emotionCheckInsServiceImpl.getById(EmotionCheckInIdMessage.newBuilder().setId(1).build(), responseObserver);

    // Verify that the onError method is called with the right status
    verify(responseObserver, times(1)).onError(any());
  }

  @Test
  public void testGetById_Exception() throws SQLException {
    // Simulate SQLException
    when(emotionCheckInsDAO.GetSingle(1)).thenThrow(new SQLException("Database error"));

    // Call method
    emotionCheckInsServiceImpl.getById(EmotionCheckInIdMessage.newBuilder().setId(1).build(), responseObserver);

    // Verify that the error response was sent
    verify(responseObserver, times(1)).onError(any());
  }

  @Test
  public void testCreateEmotionCheckIn_Success() throws SQLException {
    // Prepare mock data
    EmotionCheckInCreateMessage request = EmotionCheckInCreateMessage.newBuilder()
        .setEmotion("Sad")
        .setUserId(123)
        .setDescription("Feeling down")
        .build();

    EmotionCheckIn mockNewEmotionCheckIn = new EmotionCheckIn();
    mockNewEmotionCheckIn.setEmotion("Sad");
    mockNewEmotionCheckIn.setUserId(123);
    mockNewEmotionCheckIn.setDescription("Feeling down");

    // Mock DAO call
    when(emotionCheckInsDAO.Create(any(EmotionCheckIn.class), anyList())).thenReturn(mockNewEmotionCheckIn);

    // Mock responseObserver behavior
    EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
    emotionCheckInBuilder.setEmotion(mockNewEmotionCheckIn.getEmotion())
        .setUserId(mockNewEmotionCheckIn.getUserId())
        .setDescription(mockNewEmotionCheckIn.getDescription());

    // Call create
    emotionCheckInsServiceImpl.create(request, responseObserver);

    // Verify that the responseObserver was called correctly
    verify(responseObserver, times(1)).onNext(emotionCheckInBuilder.build());
    verify(responseObserver, times(1)).onCompleted();
  }

  @Test
  public void testDeleteEmotionCheckIn_Success() throws SQLException {
    // Prepare mock data
    EmotionCheckInIdMessage request = EmotionCheckInIdMessage.newBuilder().setId(1).build();
    EmotionCheckIn mockEmotionCheckIn = new EmotionCheckIn();
    mockEmotionCheckIn.setId(1);
    mockEmotionCheckIn.setEmotion("Happy");

    // Mock DAO call
    when(emotionCheckInsDAO.GetSingle(1)).thenReturn(mockEmotionCheckIn);

    // Call delete
    emotionCheckInsServiceImpl.delete(request, responseObserver);

    // Verify delete
    verify(emotionCheckInsDAO, times(1)).Delete(1);
    verify(responseObserver, times(1)).onNext(any());
    verify(responseObserver, times(1)).onCompleted();
  }
}
