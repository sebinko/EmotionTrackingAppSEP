using API.DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Protobuf.Services;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionCheckInsController(EmotionCheckInService emotionCheckInService, UsersService usersService) : ControllerBase
{
    // Endpoint to create a new emotion check-in
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] EmotionCheckInDTO emotionCheckInDTO)
    { 

        var emotionCheckIn = new EmotionCheckIn
        {
            UserId = emotionCheckInDTO.UserId,
            Emotion = emotionCheckInDTO.Emotion,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        emotionCheckIn = await emotionCheckInService.Create(emotionCheckIn);

        return Ok(new EmotionCheckInReturnDTO
        {
            Id = emotionCheckIn.Id,
            UserId = emotionCheckIn.UserId,
            Emotion = emotionCheckIn.Emotion,
            CreatedAt = emotionCheckIn.CreatedAt,
            UpdatedAt = emotionCheckIn.UpdatedAt
        });
    }
    
}
