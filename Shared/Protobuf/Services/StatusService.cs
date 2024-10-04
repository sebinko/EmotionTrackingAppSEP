using Grpc.Core;
using Grpc.Net.Client;

namespace Protobuf.Services;

public class StatusService
{
    public void GetStatusMethod()
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:8888");
            var client = new Protobuf.StatusService.StatusServiceClient(channel);
            var reply = client.GetStatusMethod(new Protobuf.StatusRequest());

            if (reply.Status != "OK")
            {
                throw new Exception($"{reply.Message} : {reply.Data}");
            }
        }
        catch (RpcException e)
        {
            if (e.StatusCode == StatusCode.Unavailable)
            {
                throw new Exception("JavaDAO: Service is unavailable");
            }

            throw new Exception($"JavaDAO: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"JavaDAO: {e.Message}");
        }
    }
}