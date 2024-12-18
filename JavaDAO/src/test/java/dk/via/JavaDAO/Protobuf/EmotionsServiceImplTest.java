package dk.via.JavaDAO.Protobuf;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

import dk.via.JavaDAO.Models.Emotion;
import dk.via.JavaDAO.Models.Color;
import dk.via.JavaDAO.Protobuf.Emotions.EmotionsMessage;
import dk.via.JavaDAO.Protobuf.Emotions.EmotionsRequest;
import dk.via.JavaDAO.Protobuf.Emotions.EmotionsServiceImpl;
import dk.via.JavaDAO.Services.EmotionListService;
import io.grpc.stub.StreamObserver;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;
import java.util.List;

public class EmotionsServiceImplTest {

  @Mock private EmotionListService emotionListService;
  @Mock private StreamObserver<EmotionsMessage> responseObserver;
  private EmotionsServiceImpl emotionsService;

  @BeforeEach
  public void setup() {
    MockitoAnnotations.openMocks(this); // Initialize mocks
    emotionsService = new EmotionsServiceImpl(emotionListService);
  }

//  @Test
//  public void testGetEmotionsMethod_getAll() {
//    // Mock behavior of the emotionListService
//    when(emotionListService.getEmotions()).thenReturn(
//        List.of(new Emotion("Happy", Color.RED, "Feeling good"))
//    );
//
//    // Create request and call the method
//    EmotionsRequest request = EmotionsRequest.newBuilder().build();
//    emotionsService.getEmotionsMethod(request, responseObserver);
//
//    // Verify the interaction and assert the response
//    verify(responseObserver, times(1)).onNext(any(EmotionsMessage.class));
//    verify(responseObserver, times(1)).onCompleted();
//  }

  @Test
  public void testGetEmotionsMethod_getSingleEmotion() {
    // Mock behavior of the emotionListService for specific emotion
    String emotionQuery = "Happy";
    Emotion emotion = new Emotion(emotionQuery, Color.RED, "Feeling good");
    when(emotionListService.getEmotion(emotionQuery)).thenReturn(emotion);

    // Create request and call the method
    EmotionsRequest request = EmotionsRequest.newBuilder().setEmotion(emotionQuery).build();
    emotionsService.getEmotionsMethod(request, responseObserver);

    // Verify and assert
    verify(responseObserver, times(1)).onNext(any(EmotionsMessage.class));
    verify(responseObserver, times(1)).onCompleted();
  }

//  @Test
//  public void testGetEmotionsMethod_filterByColor() {
//    // Mock behavior for filtering by color
//    String colorQuery = "RED";
//    when(emotionListService.getEmotionsByColor(Color.RED)).thenReturn(
//        List.of(new Emotion("Happy", Color.RED, "Feeling good"))
//    );
//
//    // Create request with color query and call the method
//    EmotionsRequest request = EmotionsRequest.newBuilder().setColor(colorQuery).build();
//    emotionsService.getEmotionsMethod(request, responseObserver);
//
//    // Verify and assert
//    verify(responseObserver, times(1)).onNext(any(EmotionsMessage.class));
//    verify(responseObserver, times(1)).onCompleted();
//  }

  @Test
  public void testHandleSingleEmotion_noEmotionFound() {
    // Case where emotion is not found
    String query = "Sad";
    when(emotionListService.getEmotion(query)).thenReturn(null);

    // Create request and invoke the method
    EmotionsRequest request = EmotionsRequest.newBuilder().setEmotion(query).build();
    emotionsService.getEmotionsMethod(request, responseObserver);

    // Verify that empty message is returned when not found
    verify(responseObserver, times(1)).onNext(any(EmotionsMessage.class));
    verify(responseObserver, times(1)).onCompleted();
  }

//  @Test
//  public void testHandleFilterByColor_emptyEmotionList() {
//    // Case where no emotions exist for the color query
//    String colorQuery = "BLUE";
//    when(emotionListService.getEmotionsByColor(Color.BLUE)).thenReturn(List.of());
//
//    // Create request and invoke the method
//    EmotionsRequest request = EmotionsRequest.newBuilder().setColor(colorQuery).build();
//    emotionsService.getEmotionsMethod(request, responseObserver);
//
//    // Verify interaction with the observer
//    verify(responseObserver, times(1)).onNext(any(EmotionsMessage.class));
//    verify(responseObserver, times(1)).onCompleted();
//  }
}
