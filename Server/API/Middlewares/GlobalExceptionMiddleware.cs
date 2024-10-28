using System.Text.Json;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

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
      if (ex.StatusCode == StatusCode.NotFound)
      {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
      }
      else
      {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      }
      
      await context.Response.WriteAsync(GetJsonString(ex));
    }
    catch (Exception ex)
    {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsync(GetJsonString(ex));
    }
  }
    
  private String GetJsonString(Exception ex)
  {
    return JsonSerializer.Serialize(new { error = ex.Message });
  }
}