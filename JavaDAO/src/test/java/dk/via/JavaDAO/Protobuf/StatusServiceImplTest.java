package dk.via.JavaDAO.Protobuf;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

import com.google.inject.Inject;
import dk.via.JavaDAO.Status.StatusServiceGrpc.StatusServiceImplBase;
import dk.via.JavaDAO.Status.StatusServiceImpl;
import dk.via.JavaDAO.Util.Interfaces.DBConnector;
import dk.via.JavaDAO.Status.StatusMessage;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;

import java.sql.Connection;

public class StatusServiceImplTest {

  @Mock
  private DBConnector dbConnector;

  @Mock
  private Connection connection;

  @Mock
  private StreamObserver<StatusMessage> responseObserver;

  private StatusServiceImpl statusService;

  @BeforeEach
  public void setUp() {
    MockitoAnnotations.openMocks(this); // Initialize mocks
    statusService = new StatusServiceImpl(dbConnector);
  }

  @Test
  public void testGetStatusMethod_DatabaseConnected() {
    // Arrange
    when(dbConnector.getConnection()).thenReturn(connection); // Simulate a successful database connection

    // Act
    statusService.getStatusMethod(null, responseObserver); // Call method under test

    // Assert
    verify(dbConnector, times(1)).getConnection(); // Verify the connection method was called
    verify(responseObserver, times(1)).onNext(argThat(statusMessage ->
        "OK".equals(statusMessage.getStatus()) &&
            "CLIENT RUNNING, DATABASE CONNECTED".equals(statusMessage.getMessage()))); // Check if the right message is returned
    verify(responseObserver, times(1)).onCompleted(); // Verify onCompleted was called
    verify(responseObserver, never()).onError(any()); // Ensure no error occurred
  }

  @Test
  public void testGetStatusMethod_DatabaseNotConnected() {
    // Arrange
    when(dbConnector.getConnection()).thenReturn(null); // Simulate no database connection

    // Act
    statusService.getStatusMethod(null, responseObserver); // Call method under test

    // Assert
    verify(dbConnector, times(1)).getConnection(); // Verify the connection method was called
    verify(responseObserver, times(1)).onNext(argThat(statusMessage ->
        "NOT OK".equals(statusMessage.getStatus()) &&
            "CLIENT RUNNING, BUT DATABASE NOT CONNECTED".equals(statusMessage.getMessage()))); // Check if the correct failure message is returned
    verify(responseObserver, times(1)).onCompleted(); // Verify onCompleted was called
    verify(responseObserver, never()).onError(any()); // Ensure no error occurred
  }
}
