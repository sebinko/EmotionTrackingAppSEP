package dk.via.JavaDAO.Protobuf.EmotionCheckIns;

import dk.via.JavaDAO.DAO.EmotionCheckInsDAO;
import dk.via.JavaDAO.Protobuf.EmotionCheckIns.EmotionCheckInsServiceGrpc.EmotionCheckInsServiceImplBase;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import jakarta.inject.Inject;

public class EmotionCheckInsServiceImpl extends EmotionCheckInsServiceImplBase {

  private final EmotionCheckInsDAO emotionCheckInsDAO;

  @Inject
  public EmotionCheckInsServiceImpl(EmotionCheckInsDAO emotionCheckInsDAO) {
    super();

    this.emotionCheckInsDAO = emotionCheckInsDAO;
  }

  @Override
  public void getAll(GetAllEmotionCheckInsMessage request,
      StreamObserver<ListEmotionCheckInMessage> responseObserver) {
    ListEmotionCheckInMessage.Builder listEmotionCheckInBuilder = ListEmotionCheckInMessage.newBuilder();

    emotionCheckInsDAO.GetAll(request.getUserId()).forEach(emotionCheckIn -> {
      EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
      emotionCheckInBuilder.setEmotion(emotionCheckIn.getEmotion());
      emotionCheckInBuilder.setDescription(emotionCheckIn.getDescription());
      emotionCheckInBuilder.setId(emotionCheckIn.getId());
      emotionCheckInBuilder.setCreatedAt(emotionCheckIn.getCreatedAt());
      emotionCheckInBuilder.setUpdatedAt(emotionCheckIn.getUpdatedAt());
      emotionCheckInBuilder.setUserId(emotionCheckIn.getUserId());
      listEmotionCheckInBuilder.addEmotionCheckIns(emotionCheckInBuilder.build());
    });

    responseObserver.onNext(listEmotionCheckInBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void create(EmotionCheckInCreateMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    dk.via.JavaDAO.Models.EmotionCheckIn newEmotionCheckIn = new dk.via.JavaDAO.Models.EmotionCheckIn();
    newEmotionCheckIn.setEmotion(request.getEmotion());
    newEmotionCheckIn.setUserId((request.getUserId()));
    newEmotionCheckIn.setDescription(request.getDescription());
    newEmotionCheckIn = emotionCheckInsDAO.Create(newEmotionCheckIn, null);
    EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
    emotionCheckInBuilder.setEmotion(newEmotionCheckIn.getEmotion());
    emotionCheckInBuilder.setId(newEmotionCheckIn.getId());
    emotionCheckInBuilder.setCreatedAt(newEmotionCheckIn.getCreatedAt());
    emotionCheckInBuilder.setUpdatedAt(newEmotionCheckIn.getUpdatedAt());
    emotionCheckInBuilder.setUserId(newEmotionCheckIn.getUserId());
    emotionCheckInBuilder.setDescription(newEmotionCheckIn.getDescription());

    responseObserver.onNext(emotionCheckInBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void update(EmotionCheckInUpdateMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    dk.via.JavaDAO.Models.EmotionCheckIn existingEmotionCheckIn = emotionCheckInsDAO.GetSingle(request.getId());

    if (existingEmotionCheckIn == null) {
      responseObserver.onError(Status.fromCode(Status.Code.NOT_FOUND)
          .withDescription("Emotion check-in not found")
          .asRuntimeException());
      return;
    }

    if (existingEmotionCheckIn.getUserId() != request.getUserId()) {
      responseObserver.onError(Status.fromCode(Status.Code.PERMISSION_DENIED)
          .withDescription("User does not have permission to update this emotion check-in")
          .asRuntimeException());
      return;
    }

    existingEmotionCheckIn.setEmotion(request.getEmotion());
    existingEmotionCheckIn.setDescription(request.getDescription());
    emotionCheckInsDAO.Update(existingEmotionCheckIn,null,null);

    EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
    emotionCheckInBuilder.setEmotion(request.getEmotion());
    emotionCheckInBuilder.setDescription(existingEmotionCheckIn.getDescription());
    emotionCheckInBuilder.setId(existingEmotionCheckIn.getId());
    emotionCheckInBuilder.setUpdatedAt(existingEmotionCheckIn.getUpdatedAt());
    emotionCheckInBuilder.setUserId(existingEmotionCheckIn.getUserId());
    emotionCheckInBuilder.setCreatedAt(existingEmotionCheckIn.getCreatedAt());

    responseObserver.onNext(emotionCheckInBuilder.build());
    responseObserver.onCompleted();

  }

  @Override
  public void delete(EmotionCheckInIdMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    dk.via.JavaDAO.Models.EmotionCheckIn emotionCheckInToDelete = emotionCheckInsDAO.GetSingle(
        request.getId());

    if (emotionCheckInToDelete == null) {
      responseObserver.onError(Status.fromCode(Status.Code.NOT_FOUND)
          .withDescription("Emotion check-in not found")
          .asRuntimeException());
      return;
    }
    
    emotionCheckInsDAO.Delete(request.getId());

    responseObserver.onNext(EmotionCheckInMessage
        .newBuilder()
        .setId(emotionCheckInToDelete.getId())
        .setEmotion(emotionCheckInToDelete.getEmotion())
        .setCreatedAt(emotionCheckInToDelete.getCreatedAt())
        .setUpdatedAt(emotionCheckInToDelete.getUpdatedAt())
        .setUserId(emotionCheckInToDelete.getUserId())
        .build()
    );
    responseObserver.onCompleted();
  }
}
