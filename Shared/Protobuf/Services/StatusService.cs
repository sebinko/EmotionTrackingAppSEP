using Grpc.Net.Client;

namespace Protobuf.Services;

public class StatusService
{
    public void GetStatusMethod()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:8888");
        var client = new Protobuf.StatusService.StatusServiceClient(channel);
        var reply = client.GetStatusMethod(new Protobuf.StatusRequest());

        if (reply.Status != "OK")
        {
            throw new Exception($"JavaDAO: {reply.Message} : {reply.Data}");
        }
    }
    
}