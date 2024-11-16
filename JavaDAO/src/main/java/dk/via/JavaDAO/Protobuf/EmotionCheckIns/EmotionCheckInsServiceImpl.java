package dk.via.JavaDAO.Protobuf.EmotionCheckIns;

import dk.via.JavaDAO.DAO.EmotionCheckInsDAO;
import dk.via.JavaDAO.Protobuf.EmotionCheckIns.EmotionCheckInsServiceGrpc.EmotionCheckInsServiceImplBase;
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
  public void create(EmotionCheckInCreate request,
      StreamObserver<EmotionCheckIn> responseObserver) {
    dk.via.JavaDAO.Models.EmotionCheckIn newEmotionCheckIn = new dk.via.JavaDAO.Models.EmotionCheckIn();
    newEmotionCheckIn.setEmotion(request.getEmotionToUpdate().getEmotion());
    newEmotionCheckIn.setUserId(Integer.parseInt(request.getEmotionToUpdate().getUserId()));
    newEmotionCheckIn = emotionCheckInsDAO.Create(newEmotionCheckIn, null);
    EmotionCheckIn.Builder emotionCheckInBuilder = EmotionCheckIn.newBuilder();
    emotionCheckInBuilder.setEmotion(newEmotionCheckIn.getEmotion().toString());
    emotionCheckInBuilder.setId(newEmotionCheckIn.getId().toString());
    emotionCheckInBuilder.setCreatedAt(newEmotionCheckIn.getCreatedAt().toString());
    emotionCheckInBuilder.setUpdatedAt(newEmotionCheckIn.getUpdatedAt().toString());
    emotionCheckInBuilder.setUserId(newEmotionCheckIn.getUserId().toString());

    responseObserver.onNext(emotionCheckInBuilder.build());
    responseObserver.onCompleted();
  }

  @Override
  public void delete(EmotionCheckIn request,
      StreamObserver<EmotionCheckIn> responseObserver) {
    dk.via.JavaDAO.Models.EmotionCheckIn emotionCheckInToDelete = emotionCheckInsDAO.GetSingle(Integer.parseInt(request.getId()));
    emotionCheckInsDAO.Delete(emotionCheckInToDelete);
    responseObserver.onNext(request);
    responseObserver.onCompleted();
  }
}
