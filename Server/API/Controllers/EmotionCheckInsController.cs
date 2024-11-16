using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionCheckInsController(EmotionCheckInService emotionCheckInService) : ControllerBase
{
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] EmotionCheckInCreateDTO emotionCheckInDto)
  {
    return Ok(await emotionCheckInService.Create(emotionCheckInDto));
  }
  
  [HttpDelete]
  public async Task<IActionResult> Delete([FromBody] int id)
  {
    return Ok(await emotionCheckInService.Delete(id));
  }
}