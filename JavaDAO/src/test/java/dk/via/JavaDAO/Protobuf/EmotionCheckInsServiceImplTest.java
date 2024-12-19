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
}
