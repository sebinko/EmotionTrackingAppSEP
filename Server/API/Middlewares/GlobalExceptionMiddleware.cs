using System.Text.Json;
using Grpc.Core;
using SharedUtil;

namespace API.Middlewares;

public class GlobalExceptionMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (RpcException ex)
    {
      context.Response.ContentType = "application/json";
      if (ex.StatusCode == StatusCode.NotFound)
        context.Response.StatusCode = StatusCodes.Status404NotFound;
      else if (ex.StatusCode == StatusCode.InvalidArgument)
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
      else if (ex.StatusCode == StatusCode.Unavailable)
        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
      else
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

      await context.Response.WriteAsync(GetJsonString(ex));
    }
    catch (Exception ex)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsync(GetJsonString(ex));
    }
  }

  private string GetJsonString(Exception ex)
  {
    return JsonSerializer.Serialize(new ApiExceptionResponse
    {
      Error = ex.Message,
      Trace = ex.StackTrace,
      Type = ex.GetType().Name
    });
  }

  private string GetJsonString(RpcException ex)
  {
    return JsonSerializer.Serialize(new ApiExceptionResponse
    {
      Error = ex.Status.Detail,
      Type = ex.Status.StatusCode.ToString(),
      Trace = ex.StackTrace
    });
  }
}