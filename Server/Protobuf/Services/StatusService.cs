using Grpc.Core;
using Grpc.Net.Client;
using Protobuf.Status;

namespace Protobuf.Services;

public class StatusService 
{
  public void GetStatusMethod()
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:8888");
    var client = new Status.StatusService.StatusServiceClient(channel);
    var reply = client.GetStatusMethod(new StatusRequest());

    if (reply.Status != "OK") throw new Exception($"{reply.Message} : {reply.Data}");
  }
}