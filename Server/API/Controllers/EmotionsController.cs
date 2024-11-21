using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionsController (EmotionsService emotionsService): ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> Get(string? emotionQuery, string? emotionColor )
  {
    return Ok(await emotionsService.GetAll(emotionQuery, emotionColor));
  }
}