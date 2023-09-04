using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Models;
using ScienceTrack.Services;

namespace ScienceTrack.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        private ArchiveService archive;
        public ArchiveController(ArchiveService archive)
        {
            this.archive = archive;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPendingGames(int pageNum = 1, int pageSize = 10)
        {
            return Ok(await archive.GetPendingGames(Response, pageNum, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetActiveGames(int pageNum = 1, int pageSize = 10)
        {
            return Ok(await archive.GetActiveGames(Response, pageNum, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetArchivedGames(int pageNum = 1, int pageSize = 10)
        {
            return Ok(await archive.GetArchivedGames(Response, pageNum, pageSize));
        }
    }
}
