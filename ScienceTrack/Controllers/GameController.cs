using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateGame()
            {
            return Ok(await game.CreateGame());
        }

        [HttpPost]
        public async Task<IActionResult> StartGame(int gameId)
        {
            return Ok(await game.StartGame(gameId));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoundUsers(int roundId)
        {
            return Ok(await game.GetRoundUsers(roundId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(int gameId, int userId)
        {
            return Ok(await game.AddUser(gameId, userId));
        }

        [HttpPost]
        public async Task<IActionResult> NextRound(int gameId, int roundId)
        {
            return Ok(await game.NextRound(gameId, roundId));
        }

        [HttpPost]
        public async Task<IActionResult> PlayerChoose(int roundId, int userId, int localSolutionId)
        {
            return Ok(await game.PlayerChoose(roundId, userId, localSolutionId));
        }

        [HttpGet]
        public async Task<IActionResult> GetScoreTable(int gameId)
        {
            return Ok(await game.GetScore(gameId));
        }
    }
}
