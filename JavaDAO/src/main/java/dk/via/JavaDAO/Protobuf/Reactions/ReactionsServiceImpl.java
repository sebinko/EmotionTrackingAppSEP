package dk.via.JavaDAO.Protobuf.Reactions;

import dk.via.JavaDAO.DAO.ReactionsDAO;
import dk.via.JavaDAO.Models.Reaction;
import dk.via.JavaDAO.Protobuf.Reactions.ReactionServiceGrpc.ReactionServiceImplBase;
import io.grpc.stub.StreamObserver;
import jakarta.inject.Inject;

public class ReactionsServiceImpl extends ReactionServiceImplBase {
  private final ReactionsDAO reactionsDAO;

  @Inject
  public ReactionsServiceImpl(ReactionsDAO reactionDAO) {
    this.reactionsDAO = reactionDAO;
  }

  @Override
  public void create(ReactionCreateMessage request,
      StreamObserver<ReactionMessage> responseObserver) {

    dk.via.JavaDAO.Models.Reaction newReaction = new dk.via.JavaDAO.Models.Reaction();
    newReaction.setEmoji(request.getEmoji());
    newReaction.setUserId(request.getUserId());
    newReaction.setEmotionCheckinId(request.getEmotionCheckInId());

    newReaction = reactionsDAO.Create(newReaction);

    ReactionMessage.Builder reactionBuilder = ReactionMessage.newBuilder();
    reactionBuilder.setEmoji(newReaction.getEmoji());
    reactionBuilder.setCreatedAt(newReaction.getCreatedAt());
    reactionBuilder.setUserId(newReaction.getUserId());
    reactionBuilder.setEmotionCheckInId(newReaction.getEmotionCheckinId());

    responseObserver.onNext(reactionBuilder.build());
    responseObserver.onCompleted();
  }
}
