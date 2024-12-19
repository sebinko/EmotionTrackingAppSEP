package dk.via.JavaDAO.Protobuf;

import dk.via.JavaDAO.DAO.TagsDAO;
import dk.via.JavaDAO.Protobuf.Users.TagsList;
import dk.via.JavaDAO.Protobuf.Users.UserId;
import dk.via.JavaDAO.Protobuf.Users.UserTagsServiceImpl;
import org.junit.Before;
import org.junit.Test;
import org.mockito.*;
import io.grpc.stub.StreamObserver;
import java.sql.SQLException;
import java.util.Arrays;
import java.util.List;

import static org.mockito.Mockito.*;

public class UserTagsServiceImplTest {

  private UserTagsServiceImpl userTagsServiceImpl;
  @Mock
  private TagsDAO tagsDAO;
  @Mock
  private StreamObserver<TagsList> responseObserver;

  @Before
  public void setUp() {
    MockitoAnnotations.openMocks(this);  // Initialize mocks
    userTagsServiceImpl = new UserTagsServiceImpl(tagsDAO);  // Pass the mocked DAO to the service
  }

  @Test
  public void testGetAllTags_DatabaseError() throws SQLException {
    // Arrange: Mock request and simulate a SQLException
    UserId request = UserId.newBuilder().setId(1).build();

    when(tagsDAO.GetAllForUser(1)).thenThrow(new SQLException("Database error"));

    // Act: Call the method under test
    userTagsServiceImpl.getAllTags(request, responseObserver);

    // Assert: Verify that an error response is sent
    verify(responseObserver, times(1)).onError(any(Exception.class)); // Ensure onError was called with an exception
  }

  @Test
  public void testGetAllTags_EmptyTags() throws Exception {
    // Arrange: Mock request and simulate an empty list from the DAO
    UserId request = UserId.newBuilder().setId(1).build();
    List<dk.via.JavaDAO.Models.Tag> tagsList = Arrays.asList();  // Empty list

    when(tagsDAO.GetAllForUser(1)).thenReturn(tagsList);  // Mock the DAO to return an empty list

    // Act: Call the method under test
    userTagsServiceImpl.getAllTags(request, responseObserver);

    // Assert: Ensure no tags are added to the response
    verify(responseObserver, times(1)).onNext(any(TagsList.class)); // Ensure onNext was called
    verify(responseObserver, times(1)).onCompleted(); // Ensure onCompleted was called
  }
}
