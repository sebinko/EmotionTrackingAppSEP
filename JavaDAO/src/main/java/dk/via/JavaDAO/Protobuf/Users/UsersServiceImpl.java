package dk.via.JavaDAO.Protobuf.Users;

import dk.via.JavaDAO.DAO.UsersDAO;
import dk.via.JavaDAO.Main;
import dk.via.JavaDAO.Protobuf.Users.UsersServiceGrpc.UsersServiceImplBase;
import dk.via.JavaDAO.Util.PasswordHasherUtil;
import io.grpc.stub.StreamObserver;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.lang.reflect.Array;
import java.util.ArrayList;
import com.google.inject.Inject;


public class UsersServiceImpl extends UsersServiceImplBase {

  private final UsersDAO usersDAO;
  private final Logger logger = LoggerFactory.getLogger(UsersServiceImpl.class.getName());

  @Inject
  public UsersServiceImpl(UsersDAO usersDAO) {
    super();

    this.usersDAO = usersDAO;
  }

  @Override
  public void getAll(Empty request,
      StreamObserver<UserList> responseObserver) {
    ArrayList<dk.via.JavaDAO.Models.User> usersList = usersDAO.GetAll();
    UserList.Builder builder = UserList.newBuilder();
    for (dk.via.JavaDAO.Models.User user : usersList) {
      builder.addUsers(User.newBuilder().
          setId(user.getId().toString()).
          setUsername(user.getUsername()).
          setEmail(user.getEmail()).
          setCreatedAt(user.getCreatedAt()).
          setUpdatedAt(user.getUpdatedAt())
      );
    }
    responseObserver.onNext(builder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void create(User request,
      StreamObserver<User> responseObserver) {
    String password = PasswordHasherUtil.getInstance().hashPassword(request.getPassword());

    dk.via.JavaDAO.Models.User newUser = new dk.via.JavaDAO.Models.User(
        request.getUsername(),
        password,
        request.getEmail(),
        null,
        null
        );
    newUser = usersDAO.Create(newUser);
    User.Builder userBuilder = User.newBuilder();
    userBuilder.setId(newUser.getId().toString());
    userBuilder.setUsername(newUser.getUsername());
    userBuilder.setEmail(newUser.getEmail());
//    userBuilder.setCreatedAt(newUser.getCreatedAt());
//    userBuilder.setUpdatedAt(newUser.getUpdatedAt());
    responseObserver.onNext(userBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void update(User request,
      StreamObserver<User> responseObserver) {
    dk.via.JavaDAO.Models.User updatedUser = usersDAO.GetSingle(Integer.parseInt(request.getId()));
    if(!request.getUsername().isEmpty()){
      updatedUser.setUsername(request.getUsername());
    }
    if(!request.getEmail().isEmpty()){
      updatedUser.setEmail(request.getEmail());
    }
    if(!request.getPassword().isEmpty()){
      updatedUser.setPassword(PasswordHasherUtil.getInstance().hashPassword(request.getPassword()));
    }

    usersDAO.Update(updatedUser);
    responseObserver.onNext(request);
    responseObserver.onCompleted();
  }




  @Override
  public void delete(User request,
      StreamObserver<User> responseObserver) {
    dk.via.JavaDAO.Models.User userToDelete = usersDAO.GetSingle(Integer.parseInt(request.getId()));
    usersDAO.Delete(userToDelete);
    responseObserver.onNext(request);
    responseObserver.onCompleted();

  }

  @Override
  public void getById(UserId request,
      StreamObserver<User> responseObserver) {
    dk.via.JavaDAO.Models.User userById = usersDAO.GetSingle(request.getId());
    User.Builder userBuilder = User.newBuilder();
    userBuilder.setId(userById.getId().toString());
    userBuilder.setUsername(userById.getUsername());
    userBuilder.setEmail(userById.getEmail());
    userBuilder.setCreatedAt(userById.getCreatedAt());
    userBuilder.setUpdatedAt(userById.getUpdatedAt());
    responseObserver.onNext(userBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void getByUsername(Username request,
      StreamObserver<User> responseObserver) {
    ArrayList<dk.via.JavaDAO.Models.User> users  = usersDAO.GetAll();
    dk.via.JavaDAO.Models.User user = users.stream().filter(user1 -> user1.getUsername().equals(request.getUsername())).findFirst().orElse(null);
    User.Builder userBuilder = User.newBuilder();
    userBuilder.setId(user.getId().toString());
    userBuilder.setUsername(user.getUsername());
    userBuilder.setEmail(user.getEmail());
    userBuilder.setCreatedAt(user.getCreatedAt());
    userBuilder.setUpdatedAt(user.getUpdatedAt());
    responseObserver.onNext(userBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void getByUsernameAndPassword(UsernameAndPassword request,
      StreamObserver<User> responseObserver) {
    ArrayList<dk.via.JavaDAO.Models.User> users  = usersDAO.GetAll();
    for (
        dk.via.JavaDAO.Models.User user : users
    ){
      System.out.println(PasswordHasherUtil.getInstance().hashPassword(request.getPassword()));
    }
    System.out.println();
    dk.via.JavaDAO.Models.User user = users.stream().
        filter(user1 -> user1.getUsername().equals(request.getUsername())).filter(user1 -> user1.getPassword().equals(PasswordHasherUtil.getInstance().hashPassword(request.getPassword()))).findFirst().orElse(null);
    User.Builder userBuilder = User.newBuilder();
    userBuilder.setId(user.getId().toString());
    userBuilder.setUsername(user.getUsername());
    userBuilder.setEmail(user.getEmail());
    userBuilder.setCreatedAt(user.getCreatedAt());
    userBuilder.setUpdatedAt(user.getUpdatedAt());
    responseObserver.onNext(userBuilder.build());
    responseObserver.onCompleted();
  }
}
