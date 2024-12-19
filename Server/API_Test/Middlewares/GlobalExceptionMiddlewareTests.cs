using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using API.Middlewares;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SharedUtil;

[TestFixture]
public class GlobalExceptionMiddlewareTests
{
    private GlobalExceptionMiddleware middleware;

    [SetUp]
    public void Setup()
    {
        middleware = new GlobalExceptionMiddleware();
    }

    [Test]
    public async Task InvokeAsync_ShouldSetNotFoundResponse_ForRpcException()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var rpcException = new RpcException(new Status(StatusCode.NotFound, "Resource not found"));

        var next = new Mock<RequestDelegate>();
        next.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Throws(rpcException);
        
        await middleware.InvokeAsync(context, next.Object);
        
        Assert.AreEqual(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.AreEqual("application/json", context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiExceptionResponse>(response);

        Assert.AreEqual("Resource not found", apiResponse.Error);
        Assert.AreEqual(StatusCode.NotFound.ToString(), apiResponse.Type);
    }

    [Test]
    public async Task InvokeAsync_ShouldSetInternalServerErrorResponse_ForGenericException()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var exception = new Exception("Unexpected error occurred");

        var next = new Mock<RequestDelegate>();
        next.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Throws(exception);
        
        await middleware.InvokeAsync(context, next.Object);
        
        Assert.AreEqual(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.AreEqual("application/json", context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiExceptionResponse>(response);

        Assert.AreEqual("Unexpected error occurred", apiResponse.Error);
        Assert.AreEqual(nameof(Exception), apiResponse.Type);
    }
}
