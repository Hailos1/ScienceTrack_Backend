using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Models;
using ScienceTrack.Services;

namespace ScienceTrack.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private ImportService service;
        public ImportController(ImportService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ImportUsers(IFormFile csv)
        {
            await service.ImportCsvUsers(csv);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ClearUsers()
        {
            await service.ClearUsers();
            return Ok();
        }
    }
}
