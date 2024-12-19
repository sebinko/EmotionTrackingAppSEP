package dk.via.JavaDAO.Protobuf.Emotions;

import com.google.inject.Inject;
import dk.via.JavaDAO.Main;
import dk.via.JavaDAO.Protobuf.Emotions.EmotionsServiceGrpc.EmotionsServiceImplBase;
import dk.via.JavaDAO.Services.EmotionListService;
import io.grpc.stub.StreamObserver;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class EmotionsServiceImpl extends EmotionsServiceImplBase {

  private final EmotionListService emotionListService;
  private final Logger logger = LoggerFactory.getLogger(Main.class.getName());


  @Inject
  public EmotionsServiceImpl(EmotionListService emotionListService) {
    super();

    this.emotionListService = emotionListService;
  }

  @Override
  public void getEmotionsMethod(EmotionsRequest request,
      StreamObserver<EmotionsMessage> responseObserver) {

    if (
        request.hasField(EmotionsRequest.getDescriptor().findFieldByName("emotion")) &&
            !request.getEmotion().isEmpty()
    ) {
      handleSingleEmotion(request, responseObserver);
    } else if (request.hasField(EmotionsRequest.getDescriptor().findFieldByName("color"))
        && !request.getColor().isEmpty()) {
      handleFilterByColor(request, responseObserver);
    } else {
      EmotionsMessage.Builder builder = EmotionsMessage.newBuilder();

      emotionListService.getEmotions().forEach(e -> {
        builder.addEmotions(
            Emotion.newBuilder().setEmotion(e.getEmotion()).setColor(e.getColor().toString())
                .setDescription(e.getDescription()));
      });

      responseObserver.onNext(builder.build());
      responseObserver.onCompleted();
    }
  }

  private void handleSingleEmotion(EmotionsRequest request,
      StreamObserver<EmotionsMessage> responseObserver) {
    logger.info("GetEmotionsMethod - GET SINGLE");

    String query = request.getEmotion();
    dk.via.JavaDAO.Models.Emotion emotion = emotionListService.getEmotion(query);

    if (emotion != null) {
      responseObserver.onNext(EmotionsMessage.newBuilder().addEmotions(
              Emotion.newBuilder().setEmotion(emotion.getEmotion())
                  .setColor(emotion.getColor().toString()).setDescription(emotion.getDescription()))
          .build());
    } else {
      responseObserver.onNext(EmotionsMessage.newBuilder().build());
    }

    responseObserver.onCompleted();

  }

  private void handleFilterByColor(EmotionsRequest request,
      StreamObserver<EmotionsMessage> responseObserver) {
    logger.info("GetEmotionsMethod - GET BY COLOR");

    String query = request.getColor();
    dk.via.JavaDAO.Models.Color color = dk.via.JavaDAO.Models.Color.valueOf(query.toUpperCase());

    EmotionsMessage.Builder builder = EmotionsMessage.newBuilder();

    emotionListService.getEmotionsByColor(color).forEach(e -> {
      builder.addEmotions(
          Emotion.newBuilder().setEmotion(e.getEmotion()).setColor(e.getColor().toString())
              .setDescription(e.getDescription()));
    });

    responseObserver.onNext(builder.build());
    responseObserver.onCompleted();

  }
}
