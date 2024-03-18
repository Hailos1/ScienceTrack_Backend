using Microsoft.AspNetCore.Mvc;

namespace ScienceTrack.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Check()
    {
        return Ok("health");
    }
}