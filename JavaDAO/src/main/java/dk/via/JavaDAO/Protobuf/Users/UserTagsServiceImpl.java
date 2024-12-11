package dk.via.JavaDAO.Protobuf.Users;

import com.google.inject.Inject;
import dk.via.JavaDAO.DAO.TagsDAO;
import dk.via.JavaDAO.DAO.UsersDAO;
import dk.via.JavaDAO.Models.User;
import dk.via.JavaDAO.Protobuf.Users.UserTagsServiceGrpc.UserTagsServiceImplBase;
import dk.via.JavaDAO.Util.PSQLExceptionParser;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import java.sql.SQLException;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UserTagsServiceImpl extends UserTagsServiceImplBase {

  private final TagsDAO tagsDAO;
  private final UsersDAO usersDAO;
  private final Logger logger = LoggerFactory.getLogger(UsersServiceImpl.class.getName());

  @Inject
  public UserTagsServiceImpl(TagsDAO tagsDAO, UsersDAO usersDAO) {
    super();
    this.tagsDAO = tagsDAO;
    this.usersDAO = usersDAO;
  }

  @Override
  public void getAllTags(UserId request, StreamObserver<TagsList> responseObserver) {
    try {
      User user = usersDAO.GetSingle(request.getId());
      List<dk.via.JavaDAO.Models.Tag> tagsList = tagsDAO.GetAllForUser(user);

      TagsList.Builder builder = TagsList.newBuilder();
      for (dk.via.JavaDAO.Models.Tag tag : tagsList) {
        builder.addTags(Tag.newBuilder()
            .setKey(tag.getKey())
            .setType(TagType.valueOf(tag.getType().toString()))
        );
      }
      responseObserver.onNext(builder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      PSQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}
