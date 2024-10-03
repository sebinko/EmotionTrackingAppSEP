package dk.via.JavaDAO;

import dk.via.JavaDAO.Status.StatusServiceImpl;
import io.grpc.Server;
import io.grpc.ServerBuilder;

public class Main {
    public static void main(String[] args) {
        Server server = ServerBuilder.forPort(8888)
                .addService(new StatusServiceImpl())
                .build();

        try {
            server.start();
            server.awaitTermination();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
