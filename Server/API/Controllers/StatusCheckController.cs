using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

public class StatusCheckController : ControllerBase
{
    private StatusService _statusService;
    
    public StatusCheckController(StatusService statusService)
    {
        _statusService = statusService;
    }
    
    [HttpGet]
    [Route("Status")]
    public IActionResult Get()
    {
        try
        {
            _statusService.GetStatusMethod();
            return Ok("API is running");
        } 
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }
}