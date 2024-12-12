package dk.via.JavaDAO.Protobuf.Users;

import com.google.inject.Inject;
import dk.via.JavaDAO.DAO.UsersDAO;
import dk.via.JavaDAO.Protobuf.Users.UsersServiceGrpc.UsersServiceImplBase;
import dk.via.JavaDAO.Util.SQLExceptionParser;
import dk.via.JavaDAO.Util.PasswordHasherUtil;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import java.sql.SQLException;
import java.util.ArrayList;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class UsersServiceImpl extends UsersServiceImplBase {

  private final UsersDAO usersDAO;
  private final Logger logger = LoggerFactory.getLogger(UsersServiceImpl.class.getName());

  @Inject
  public UsersServiceImpl(UsersDAO usersDAO) {
    super();
    this.usersDAO = usersDAO;
  }

  @Override
  public void getAll(Empty request, StreamObserver<UserList> responseObserver) {
    try {
      ArrayList<dk.via.JavaDAO.Models.User> usersList = usersDAO.GetAll();
      UserList.Builder builder = UserList.newBuilder();
      for (dk.via.JavaDAO.Models.User user : usersList) {
        builder.addUsers(User.newBuilder()
            .setId(user.getId())
            .setUsername(user.getUsername())
            .setEmail(user.getEmail())
            .setCreatedAt(user.getCreatedAt().toString())
            .setUpdatedAt(user.getUpdatedAt().toString())
            .setStreak(user.getStreak())
        );
      }
      responseObserver.onNext(builder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void create(UserCreate request, StreamObserver<User> responseObserver) {
    try {
      String password = PasswordHasherUtil.getInstance().hashPassword(request.getPassword());
      dk.via.JavaDAO.Models.User newUser = new dk.via.JavaDAO.Models.User(
          request.getUsername(),
          password,
          request.getEmail(),
          null,
          null,
          null
      );
      newUser = usersDAO.Create(newUser);
      User.Builder userBuilder = User.newBuilder();
      userBuilder.setId(newUser.getId());
      userBuilder.setUsername(newUser.getUsername());
      userBuilder.setEmail(newUser.getEmail());
      userBuilder.setCreatedAt(newUser.getCreatedAt().toString());
      userBuilder.setUpdatedAt(newUser.getUpdatedAt().toString());

      responseObserver.onNext(userBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void update(UserUpdate request, StreamObserver<User> responseObserver) {
    try {
      dk.via.JavaDAO.Models.User updatedUser = usersDAO.GetSingle(request.getId());
      if (!request.getUsername().isEmpty()) {
        updatedUser.setUsername(request.getUsername());
      }
      if (!request.getEmail().isEmpty()) {
        updatedUser.setEmail(request.getEmail());
      }
      if (!request.getPassword().isEmpty()) {
        updatedUser.setPassword(
            PasswordHasherUtil.getInstance().hashPassword(request.getPassword()));
      }

      usersDAO.Update(updatedUser);

      updatedUser = usersDAO.GetSingle(request.getId());

      User.Builder userBuilder = User.newBuilder();
      userBuilder.setId(updatedUser.getId());
      userBuilder.setUsername(updatedUser.getUsername());
      userBuilder.setEmail(updatedUser.getEmail());
      userBuilder.setCreatedAt(updatedUser.getCreatedAt().toString());
      userBuilder.setUpdatedAt(updatedUser.getUpdatedAt().toString());
      userBuilder.setStreak(updatedUser.getStreak());
      responseObserver.onNext(userBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void delete(User request, StreamObserver<User> responseObserver) {
    try {
      dk.via.JavaDAO.Models.User userToDelete = usersDAO.GetSingle(request.getId());
      usersDAO.Delete(userToDelete);
      responseObserver.onNext(request);
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getById(UserId request, StreamObserver<User> responseObserver) {
    try {
      dk.via.JavaDAO.Models.User userById = usersDAO.GetSingle(request.getId());
      User.Builder userBuilder = User.newBuilder();
      userBuilder.setId(userById.getId());
      userBuilder.setUsername(userById.getUsername());
      userBuilder.setEmail(userById.getEmail());
      userBuilder.setCreatedAt(userById.getCreatedAt().toString());
      userBuilder.setUpdatedAt(userById.getUpdatedAt().toString());
      userBuilder.setStreak(userById.getStreak());
      responseObserver.onNext(userBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getByUsername(Username request, StreamObserver<User> responseObserver) {
    try {
      ArrayList<dk.via.JavaDAO.Models.User> users = usersDAO.GetAll();
      dk.via.JavaDAO.Models.User user = users.stream()
          .filter(user1 -> user1.getUsername().equals(request.getUsername())).findFirst()
          .orElse(null);
      User.Builder userBuilder = User.newBuilder();
      userBuilder.setId(user.getId());
      userBuilder.setUsername(user.getUsername());
      userBuilder.setEmail(user.getEmail());
      userBuilder.setCreatedAt(user.getCreatedAt().toString());
      userBuilder.setUpdatedAt(user.getUpdatedAt().toString());
      userBuilder.setStreak(user.getStreak());
      responseObserver.onNext(userBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getByUsernameAndPassword(UsernameAndPassword request,
      StreamObserver<User> responseObserver) {
    try {
      dk.via.JavaDAO.Models.User user = usersDAO.GetSingle(request.getUsername(), request.getPassword());

      User.Builder userBuilder = User.newBuilder();
      userBuilder.setId(user.getId());
      userBuilder.setUsername(user.getUsername());
      userBuilder.setEmail(user.getEmail());
      userBuilder.setCreatedAt(user.getCreatedAt().toString());
      userBuilder.setUpdatedAt(user.getUpdatedAt().toString());
      userBuilder.setStreak(user.getStreak());
      responseObserver.onNext(userBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      logger.error(e.getMessage());
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}