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

  @Test
  public void testCreate_Success() throws Exception {
    // Arrange: Mock input request and output data
    UserCreate request = UserCreate.newBuilder()
        .setUsername("newUser")
        .setPassword("password123")
        .setEmail("newuser@example.com")
        .build();

    dk.via.JavaDAO.Models.User newUser = new dk.via.JavaDAO.Models.User(
        3, "newUser", "password123", "newuser@example.com",
        new Timestamp(System.currentTimeMillis()), new Timestamp(System.currentTimeMillis()), 0);

    when(usersDAO.Create(any(dk.via.JavaDAO.Models.User.class))).thenReturn(newUser); // Mock creation

    // Act: Call the method under test

    // Assert: Verify the correct method call and response
    verify(usersDAO, times(1)).Create(any(dk.via.JavaDAO.Models.User.class)); // Verify Create was called on DAO
    verify(responseObserver, times(1)).onNext(any(UserList.class)); // Ensure response is sent
    verify(responseObserver, times(1)).onCompleted(); // Ensure onCompleted is called
  }

  @Test
  public void testGetByUsernameAndPassword_Success() throws Exception {
    // Arrange: Mock request and user data
    UsernameAndPassword request = UsernameAndPassword.newBuilder()
        .setUsername("username1")
        .setPassword("password1")
        .build();

    dk.via.JavaDAO.Models.User user = new dk.via.JavaDAO.Models.User(
        1, "username1", "password1", "email1@example.com",
        new Timestamp(System.currentTimeMillis()), new Timestamp(System.currentTimeMillis()), 5);

    when(usersDAO.GetSingle("username1", "password1")).thenReturn(user); // Simulate matching credentials

    // Act: Call the method under test

    // Assert: Verify that the correct user is returned for valid credentials
    verify(usersDAO, times(1)).GetSingle("username1", "password1"); // Ensure GetSingle with credentials is called
    verify(responseObserver, times(1)).onNext(any(UserList.class)); // Ensure user is returned
    verify(responseObserver, times(1)).onCompleted(); // Ensure onCompleted is called
  }

  @Test
  public void testGetByUsernameAndPassword_UserNotFound() throws Exception {
    // Arrange: Mock request with invalid credentials
    UsernameAndPassword request = UsernameAndPassword.newBuilder()
        .setUsername("invalidUser")
        .setPassword("wrongPassword")
        .build();

    when(usersDAO.GetSingle("invalidUser", "wrongPassword")).thenReturn(null); // Simulate user not found

    // Act: Call the method under test

    // Assert: Verify no user is returned for invalid credentials
    verify(responseObserver, times(1)).onNext(null); // No user found
    verify(responseObserver, times(1)).onCompleted(); // Ensure onCompleted is called
  }



}
