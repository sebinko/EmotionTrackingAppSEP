package dk.via.JavaDAO.Protobuf.EmotionCheckIns;

import dk.via.JavaDAO.DAO.EmotionCheckInsDAO;
import dk.via.JavaDAO.DAO.TagsDAO;
import dk.via.JavaDAO.Models.EmotionCheckIn;
import dk.via.JavaDAO.Models.TagType;
import dk.via.JavaDAO.Protobuf.EmotionCheckIns.EmotionCheckInsServiceGrpc.EmotionCheckInsServiceImplBase;
import dk.via.JavaDAO.Util.SQLExceptionParser;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import jakarta.inject.Inject;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import org.postgresql.util.PSQLException;

public class EmotionCheckInsServiceImpl extends EmotionCheckInsServiceImplBase {

  private final EmotionCheckInsDAO emotionCheckInsDAO;
  private final TagsDAO tagsDAO;

  @Inject
  public EmotionCheckInsServiceImpl(EmotionCheckInsDAO emotionCheckInsDAO, TagsDAO tagsDao) {
    super();
    this.emotionCheckInsDAO = emotionCheckInsDAO;
    this.tagsDAO = tagsDao;
  }

  @Override
  public void getById(EmotionCheckInIdMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    try {
      dk.via.JavaDAO.Models.EmotionCheckIn emotionCheckIn = emotionCheckInsDAO.GetSingle(
          request.getId());
      EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
      emotionCheckInBuilder.setEmotion(emotionCheckIn.getEmotion());
      emotionCheckInBuilder.setId(emotionCheckIn.getId());
      emotionCheckInBuilder.setCreatedAt(emotionCheckIn.getCreatedAt());
      emotionCheckInBuilder.setUpdatedAt(emotionCheckIn.getUpdatedAt());
      emotionCheckInBuilder.setUserId(emotionCheckIn.getUserId());

      List<dk.via.JavaDAO.Models.Tag> tagsList = tagsDAO.GetAllForCheckIn(emotionCheckIn);

      for (dk.via.JavaDAO.Models.Tag tag : tagsList) {
        emotionCheckInBuilder.addTags(dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag.newBuilder()
            .setKey(tag.getKey())
            .setType(
                dk.via.JavaDAO.Protobuf.EmotionCheckIns.TagType.valueOf(tag.getType().toString()))
            .build());
      }

      if (emotionCheckIn.getDescription() != null) {
        emotionCheckInBuilder.setDescription(emotionCheckIn.getDescription());
      }

      responseObserver.onNext(emotionCheckInBuilder.build());
      responseObserver.onCompleted();
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void getAll(GetAllEmotionCheckInsMessage request,
      StreamObserver<ListEmotionCheckInMessage> responseObserver) {
    try {
      ListEmotionCheckInMessage.Builder listEmotionCheckInBuilder = ListEmotionCheckInMessage.newBuilder();

      ArrayList<EmotionCheckIn> checkIns = emotionCheckInsDAO.GetAll(request.getUserId());

      for (EmotionCheckIn emotionCheckIn : checkIns) {
        EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
        emotionCheckInBuilder.setEmotion(emotionCheckIn.getEmotion());

        if (emotionCheckIn.getDescription() != null) {
          emotionCheckInBuilder.setDescription(emotionCheckIn.getDescription());
        }

        emotionCheckInBuilder.setId(emotionCheckIn.getId());
        emotionCheckInBuilder.setCreatedAt(emotionCheckIn.getCreatedAt());
        emotionCheckInBuilder.setUpdatedAt(emotionCheckIn.getUpdatedAt());
        emotionCheckInBuilder.setUserId(emotionCheckIn.getUserId());

        List<dk.via.JavaDAO.Models.Tag> tagsList = tagsDAO.GetAllForCheckIn(emotionCheckIn);

        for (dk.via.JavaDAO.Models.Tag tag : tagsList) {
          emotionCheckInBuilder.addTags(dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag.newBuilder()
              .setKey(tag.getKey())
              .setType(
                  dk.via.JavaDAO.Protobuf.EmotionCheckIns.TagType.valueOf(tag.getType().toString()))
              .build());
        }

        listEmotionCheckInBuilder.addEmotionCheckIns(emotionCheckInBuilder.build());
      }
      responseObserver.onNext(listEmotionCheckInBuilder.build());
      responseObserver.onCompleted();
    } catch (SQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void create(EmotionCheckInCreateMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    try {
      dk.via.JavaDAO.Models.EmotionCheckIn newEmotionCheckIn = new dk.via.JavaDAO.Models.EmotionCheckIn();
      newEmotionCheckIn.setEmotion(request.getEmotion());
      newEmotionCheckIn.setUserId(request.getUserId());
      newEmotionCheckIn.setDescription(request.getDescription());

      List<dk.via.JavaDAO.Models.Tag> tagsList = new ArrayList<>();

      for (dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag tag : request.getTagsList()) {
        dk.via.JavaDAO.Models.Tag newTag = new dk.via.JavaDAO.Models.Tag();
        newTag.setKey(tag.getKey());
        newTag.setType(TagType.valueOf(tag.getType().toString()));
        newTag.setUserId(request.getUserId());
        tagsList.add(newTag);
      }

      newEmotionCheckIn = emotionCheckInsDAO.Create(newEmotionCheckIn, tagsList);
      EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
      emotionCheckInBuilder.setEmotion(newEmotionCheckIn.getEmotion());
      emotionCheckInBuilder.setId(newEmotionCheckIn.getId());
      emotionCheckInBuilder.setCreatedAt(newEmotionCheckIn.getCreatedAt());
      emotionCheckInBuilder.setUpdatedAt(newEmotionCheckIn.getUpdatedAt());
      emotionCheckInBuilder.setUserId(newEmotionCheckIn.getUserId());

      for (dk.via.JavaDAO.Models.Tag tag : tagsList) {
        emotionCheckInBuilder.addTags(dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag.newBuilder()
            .setKey(tag.getKey())
            .setType(
                dk.via.JavaDAO.Protobuf.EmotionCheckIns.TagType.valueOf(tag.getType().toString()))
            .build());
      }

      if (newEmotionCheckIn.getDescription() != null) {
        emotionCheckInBuilder.setDescription(newEmotionCheckIn.getDescription());
      }

      responseObserver.onNext(emotionCheckInBuilder.build());
      responseObserver.onCompleted();
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void update(EmotionCheckInUpdateMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    try {
      dk.via.JavaDAO.Models.EmotionCheckIn existingEmotionCheckIn = emotionCheckInsDAO.GetSingle(
          request.getId());
      existingEmotionCheckIn.setEmotion(request.getEmotion());
      existingEmotionCheckIn.setDescription(request.getDescription());

      List<dk.via.JavaDAO.Models.Tag> tagsList = new ArrayList<>();

      for (dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag tag : request.getTagsList()) {
        dk.via.JavaDAO.Models.Tag newTag = new dk.via.JavaDAO.Models.Tag();
        newTag.setKey(tag.getKey());
        newTag.setType(TagType.valueOf(tag.getType().toString()));
        newTag.setUserId(request.getUserId());
        tagsList.add(newTag);
      }

      emotionCheckInsDAO.Update(existingEmotionCheckIn, tagsList);

      EmotionCheckInMessage.Builder emotionCheckInBuilder = EmotionCheckInMessage.newBuilder();
      emotionCheckInBuilder.setEmotion(request.getEmotion());
      emotionCheckInBuilder.setId(existingEmotionCheckIn.getId());
      emotionCheckInBuilder.setUpdatedAt(existingEmotionCheckIn.getUpdatedAt());
      emotionCheckInBuilder.setUserId(existingEmotionCheckIn.getUserId());
      emotionCheckInBuilder.setCreatedAt(existingEmotionCheckIn.getCreatedAt());

      for (dk.via.JavaDAO.Models.Tag tag : tagsList) {
        emotionCheckInBuilder.addTags(dk.via.JavaDAO.Protobuf.EmotionCheckIns.Tag.newBuilder()
            .setKey(tag.getKey())
            .setType(
                dk.via.JavaDAO.Protobuf.EmotionCheckIns.TagType.valueOf(tag.getType().toString()))
            .build());
      }

      if (existingEmotionCheckIn.getDescription() != null) {
        emotionCheckInBuilder.setDescription(existingEmotionCheckIn.getDescription());
      }

      responseObserver.onNext(emotionCheckInBuilder.build());
      responseObserver.onCompleted();
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }

  @Override
  public void delete(EmotionCheckInIdMessage request,
      StreamObserver<EmotionCheckInMessage> responseObserver) {
    try {
      dk.via.JavaDAO.Models.EmotionCheckIn emotionCheckInToDelete = emotionCheckInsDAO.GetSingle(
          request.getId());
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
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}