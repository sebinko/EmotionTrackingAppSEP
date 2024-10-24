package dk.via.JavaDAO.Services;

import com.opencsv.CSVParserBuilder;
import com.opencsv.CSVReader;
import com.opencsv.CSVReaderBuilder;
import dk.via.JavaDAO.Models.Color;
import dk.via.JavaDAO.Models.Emotion;
import dk.via.JavaDAO.Util.AppConfig;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import java.io.FileReader;
import java.util.HashSet;

public class EmotionListService {
  private final Logger logger = LoggerFactory.getLogger(EmotionListService.class.getName());

  private final HashSet<Emotion> emotions = new HashSet<>();

  public EmotionListService() {
    try (CSVReader csvReader = new CSVReaderBuilder(
        new FileReader("src/main/resources/data/emotions.csv")).withCSVParser(
        new CSVParserBuilder().withSeparator(';').build()).build()) {
      String[] values;
      while ((values = csvReader.readNext()) != null) {
        emotions.add(new Emotion(values[0], Color.valueOf(values[1].toUpperCase()), values[2]));
      }
    } catch (Exception e) {
      logger.error("Error reading emotions file: {}", e.getMessage());
    }
  }

  public HashSet<Emotion> getEmotions() {
    return emotions;
  }

  public Emotion getEmotion(String name) {
    return emotions.stream().filter(e -> e.getEmotion().equals(name)).findFirst()
        .orElse(null);
  }

  public HashSet<Emotion> getEmotionsByColor(Color color) {
    return emotions.stream().filter(e -> e.getColor().equals(color)).collect(
        HashSet::new, HashSet::add, HashSet::addAll);
  }
}
