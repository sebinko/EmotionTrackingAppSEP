using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionCheckInsController(EmotionCheckInService emotionCheckInService) : ControllerBase
{
  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] EmotionCheckInCreateDTO emotionCheckInDto)
  {
    return Ok(await emotionCheckInService.Create(emotionCheckInDto));
  }
}