package dk.via.JavaDAO.Protobuf;

import static org.mockito.Mockito.*;
import static org.junit.Assert.*;

import dk.via.JavaDAO.DAO.UsersDAO;
import org.junit.Before;
import org.junit.Test;
import org.mockito.*;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;

import io.grpc.stub.StreamObserver;
import dk.via.JavaDAO.Protobuf.Users.*;

public class UsersServiceImplTest {

  @Mock
  private UsersDAO usersDAO; // Mock the UsersDAO
  @Mock
  private StreamObserver<UserList> responseObserver; // Mock the StreamObserver

  private UsersServiceImpl usersServiceImpl;

  @Before
  public void setUp() {
    MockitoAnnotations.initMocks(this); // Initialize mocks
    usersServiceImpl = new UsersServiceImpl(usersDAO); // Create service instance with mocked DAO
  }

  @Test
  public void testGetAll_Success() throws Exception {
    // Arrange: Mock the DAO behavior to return a list of users
    ArrayList<dk.via.JavaDAO.Models.User> userList = new ArrayList<>();
    userList.add(new dk.via.JavaDAO.Models.User(1, "username1", "password1", "email1@example.com",
        new Timestamp(System.currentTimeMillis()), new Timestamp(System.currentTimeMillis()), 5));
    userList.add(new dk.via.JavaDAO.Models.User(2, "username2", "password2", "email2@example.com",
        new Timestamp(System.currentTimeMillis()), new Timestamp(System.currentTimeMillis()), 3));

    when(usersDAO.GetAll()).thenReturn(userList); // Simulate DAO call

    // Act: Call the method under test
    usersServiceImpl.getAll(Empty.getDefaultInstance(), responseObserver);

    // Assert: Verify the expected interactions and correct response
    verify(usersDAO, times(1)).GetAll(); // Ensure GetAll() was called on the DAO
    verify(responseObserver, times(1)).onNext(any(UserList.class)); // Verify that onNext was called on the response observer
    verify(responseObserver, times(1)).onCompleted(); // Verify that onCompleted was called
  }

  @Test
  public void testGetAll_Exception() throws Exception {
    // Arrange: Mock DAO to throw SQLException
    when(usersDAO.GetAll()).thenThrow(new SQLException("Database error"));

    // Act: Call the method under test
    usersServiceImpl.getAll(Empty.getDefaultInstance(), responseObserver);

    // Assert: Verify that the exception handling works (onError should be invoked)
    verify(responseObserver, times(1)).onError(any()); // Verify that onError was called due to SQLException
  }

}
