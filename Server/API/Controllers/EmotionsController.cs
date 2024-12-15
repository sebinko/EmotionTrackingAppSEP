using DTO;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;
using Protobuf.Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionsController (IEmotionsService emotionsService): ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> Get(string? emotionQuery, string? emotionColor )
  {
    return Ok(await emotionsService.GetAll(emotionQuery, emotionColor));
  }
}