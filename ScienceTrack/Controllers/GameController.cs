using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Models;
using ScienceTrack.Repositories;
using ScienceTrack.Services;

namespace ScienceTrack.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameService game;
        public GameController(GameService game) 
        {
            this.game = game;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGame()
        {
            return Ok(await game.CreateGame());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> StartGame([FromForm] int gameId)
        {
            return Ok(await game.StartGame(gameId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoundUsers([FromForm] int roundId)
        {
            return Ok(await game.GetRoundUsers(roundId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUser([FromForm] int gameId, [FromForm] int userId)
        {
            return Ok(await game.AddUser(gameId, userId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NextRound([FromForm] int gameId, [FromForm] int roundId)
        {
            return Ok(await game.NextRound(gameId, roundId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlayerChoose([FromForm] int roundId, [FromForm] int userId, [FromForm] int localSolutionId)
        {
            return Ok(await game.PlayerChoose(roundId, userId, localSolutionId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLastGame()
        {
            return Ok(await game.GetLastGame());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetScoreTable([FromForm] int gameId)
        {
            return Ok(await game.GetScore(gameId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSolutions([FromForm] int pageNum, [FromForm] int pageSize)
        {
            return Ok( await game.GetSolutions(Response, pageNum, pageSize));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserLocalEvent([FromForm] int roundId, [FromForm] string userName)
        {
            return Ok(await game.GetUserLocalEvent(roundId, userName));
        }
    }
}
