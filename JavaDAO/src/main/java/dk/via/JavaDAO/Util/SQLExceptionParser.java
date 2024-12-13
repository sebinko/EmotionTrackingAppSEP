package dk.via.JavaDAO.Util;

import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import java.sql.SQLException;
import org.postgresql.util.PSQLException;
import org.postgresql.util.PSQLState;

public class SQLExceptionParser {

  public static <T> void Parse(SQLException e, StreamObserver<T> responseObserver) {
    extracted(e, responseObserver);
  }

  public static <T> void Parse(PSQLException e, StreamObserver<T> responseObserver) {
    extracted(e, responseObserver);
  }

  private static <T> void extracted(SQLException e, StreamObserver<T> responseObserver) {
    // unique violation
    if (PSQLState.UNIQUE_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.ALREADY_EXISTS.withCause(e).withDescription("Entity already exists")
              .asException());
    }

    // foreign key violation
    else if (PSQLState.FOREIGN_KEY_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.NOT_FOUND.withCause(e).withDescription("Foreign Entity not found.").asException());
    }

    // check violation
    else if (PSQLState.CHECK_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.INVALID_ARGUMENT.withCause(e).withDescription(e.getMessage()).asException());
    }

    // not null violation
    else if (PSQLState.NOT_NULL_VIOLATION.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.INVALID_ARGUMENT.withCause(e).withDescription(e.getMessage()).asException());
    } else if (PSQLState.INVALID_PASSWORD.getState().equals(e.getSQLState())) {
      responseObserver.onError(
          Status.INVALID_ARGUMENT.withCause(e).withDescription(e.getMessage()).asException());
    }

    // unknown
    else {
      responseObserver.onError(
          Status.INTERNAL.withCause(e).withDescription(e.getMessage()).asException());
    }
  }
}
