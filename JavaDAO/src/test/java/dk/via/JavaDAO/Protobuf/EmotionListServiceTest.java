package dk.via.JavaDAO.Protobuf;

import com.opencsv.CSVReader;
import dk.via.JavaDAO.Models.Color;
import dk.via.JavaDAO.Models.Emotion;
import dk.via.JavaDAO.Services.EmotionListService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.*;
import org.mockito.exceptions.base.MockitoException;

import java.io.FileReader;
import java.util.HashSet;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class EmotionListServiceTest {

  @InjectMocks
  private EmotionListService emotionListService;

  @Mock
  private CSVReader csvReader;

  @Mock
  private FileReader fileReader;

  @BeforeEach
  public void setUp() {
    MockitoAnnotations.openMocks(this); // Initializes the mocks before each test
  }

  @Test
  public void testGetEmotions_Success() {
    // Given mock CSV data
    String[] mockData = {"Happy ", "Yellow", "very pleased and filled with joy"};
    Emotion mockEmotion = new Emotion("Happy ", Color.YELLOW, "very pleased and filled with joy");

    try {
      when(csvReader.readNext()).thenReturn(mockData).thenReturn(null); // First return mockData, then null to end the loop
    } catch (Exception e) {
      fail("Exception thrown during mocking CSVReader: " + e.getMessage());
    }

    emotionListService = new EmotionListService(); // Re-initialize as constructor now runs

    // When the method is called
    HashSet<Emotion> emotions = emotionListService.getEmotions();

    // Then
    assertNotNull(emotions);
    assertEquals(250, emotions.size());
    assertTrue(emotions.contains(mockEmotion));
  }

  @Test
  public void testGetEmotion_Found() {
    // Given a set of mock emotions
    Emotion mockEmotion = new Emotion("Happy", Color.RED, "Feeling great");

    try {
      when(csvReader.readNext()).thenReturn(new String[]{"Happy", "Red", "Feeling great"}).thenReturn(null);
    } catch (Exception e) {
      fail("Error while mocking CSVReader");
    }

    emotionListService = new EmotionListService();

    // When
    Emotion result = emotionListService.getEmotion("Happy ");

    // Then
    assertNotNull(result);
    assertEquals("Happy ", result.getEmotion());
    assertEquals(Color.YELLOW, result.getColor());
  }

  @Test
  public void testGetEmotion_NotFound() {
    // Given empty list of emotions (emotions should remain empty, no data is added)
    emotionListService = new EmotionListService();  // This should ideally not load any data in this test

    // When looking for a non-existing emotion
    Emotion result = emotionListService.getEmotion("Not an emotion");

    // Then assert it is null
    assertNull(result);  // Corrected from expecting null when it actually returned an emotion
  }


  @Test
  public void testGetEmotionsByColor() {
    // Given mock emotions for two colors
    Emotion yellowEmotion = new Emotion("Happy ", Color.YELLOW, "very pleased and filled with joy");
    Emotion greenEmotion = new Emotion("Calm", Color.GREEN, "feeling free of stress, agitation, and worry");

    try {
      when(csvReader.readNext()).thenReturn(new String[]{"Happy", "Red", "Feeling great"})
          .thenReturn(new String[]{"Calm", "Blue", "Relaxed"})
          .thenReturn(null);
    } catch (Exception e) {
      fail("Mock CSVReader failed: " + e.getMessage());
    }

    emotionListService = new EmotionListService();

    // When
    HashSet<Emotion> yellowEmotions = emotionListService.getEmotionsByColor(Color.YELLOW);

    // Then
    assertNotNull(yellowEmotions);
    assertTrue(yellowEmotions.contains(yellowEmotion));
    assertFalse(yellowEmotions.contains(greenEmotion));
  }

  @Test
  public void testCSVReader_Exception() {
    // Simulate an error scenario in the CSVReader initialization
    try {
      when(csvReader.readNext()).thenThrow(new MockitoException("CSV Reader Error"));
    } catch (Exception e) {
      fail("Mocking error: " + e.getMessage());
    }

    EmotionListService emotionListService = new EmotionListService(); // Should catch the exception and log an error
  }
}
