package dk.via.JavaDAO.Protobuf.Reactions;

import dk.via.JavaDAO.DAO.ReactionsDAO;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionServiceGrpc.ReactionServiceImplBase;
import dk.via.JavaDAO.Util.SQLExceptionParser;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import jakarta.inject.Inject;
import org.postgresql.util.PSQLException;

public class ReactionsServiceImpl extends ReactionServiceImplBase {
  private final ReactionsDAO reactionsDAO;

  @Inject
  public ReactionsServiceImpl(ReactionsDAO reactionDAO) {
    this.reactionsDAO = reactionDAO;
  }

  @Override
  public void create(ReactionCreateMessage request,
      StreamObserver<ReactionMessage> responseObserver) {

    try{
    dk.via.JavaDAO.Models.Reaction newReaction = new dk.via.JavaDAO.Models.Reaction();
    newReaction.setEmoji(request.getEmoji());
    newReaction.setUserId(request.getUserId());
    newReaction.setEmotionCheckinId(request.getEmotionCheckInId());

    newReaction = reactionsDAO.Create(newReaction);

    ReactionMessage.Builder reactionBuilder = ReactionMessage.newBuilder();
    reactionBuilder.setEmoji(newReaction.getEmoji());
    reactionBuilder.setCreatedAt(newReaction.getCreatedAt().toString());
    reactionBuilder.setUpdatedAt(newReaction.getUpdatedAt().toString());
    reactionBuilder.setUserId(newReaction.getUserId());
    reactionBuilder.setEmotionCheckInId(newReaction.getEmotionCheckinId());

    responseObserver.onNext(reactionBuilder.build());
    responseObserver.onCompleted();
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }

  }

  @Override
  public void delete(ReactionDeleteMessage request,
      StreamObserver<ReactionMessage> responseObserver) {
    try{


      reactionsDAO.Delete(request.getUserId(), request.getEmotionCheckInId());

      ReactionMessage.Builder reactionBuilder = ReactionMessage.newBuilder();
     

      responseObserver.onNext(reactionBuilder.build());
      responseObserver.onCompleted();
    } catch (PSQLException e) {
      SQLExceptionParser.Parse(e, responseObserver);
    } catch (Exception e) {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}
