package dk.via.JavaDAO.Util;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import org.postgresql.util.PSQLException;
import org.postgresql.util.PSQLState;

public class PSQLExceptionParser {

  public static <T> void Parse(PSQLException e, StreamObserver<T> responseObserver) {
    // unique violation
    if (PSQLState.UNIQUE_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.ALREADY_EXISTS.withCause(e).withDescription("Entity already exists").asException());
    }

    // foreign key violation
    if (PSQLState.FOREIGN_KEY_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.NOT_FOUND.withCause(e).withDescription(e.getMessage()).asException());
    }

    // check violation
    if (PSQLState.CHECK_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.INVALID_ARGUMENT.withCause(e).withDescription(e.getMessage()).asException());
    }

    // not null violation
    if (PSQLState.NOT_NULL_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.INVALID_ARGUMENT.withCause(e).withDescription(e.getMessage()).asException());
    }

    // unknown
    responseObserver.onError(Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
  }

}
