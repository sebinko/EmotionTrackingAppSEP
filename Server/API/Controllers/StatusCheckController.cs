using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StatusCheckController : ControllerBase
{
    [HttpGet]
    [Route("Status")]
    public IActionResult Get()
    {
        // TODO add logic to connect with gRPC with JavaDAO and return bad if not connected
        return Ok("API is running");
    }
}