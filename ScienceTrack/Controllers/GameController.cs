﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Services;

namespace ScienceTrack.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController(GameService game) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGame()
        {
            return Ok(await game.CreateGame());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> StartGame(int gameId)
        {
            return Ok(await game.StartGame(gameId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoundUsers(int roundId)
        {
            return Ok(await game.GetRoundUsers(roundId));
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserGraph(int gameId, int userId, string? username = null)
        {
            return Ok(await game.GetUserGraph(gameId, userId, username));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUser(int gameId, int userId)
        {
            return Ok(await game.AddUser(gameId, userId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NextRound(int gameId, int roundId)
        {
            return Ok(await game.NextRound(gameId, roundId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlayerChoose(int roundId, int userId, int localSolutionId)
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
        public async Task<IActionResult> GetScoreTable(int gameId)
        {
            return Ok(await game.GetScore(gameId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSolutions(int stage, int pageNum, int pageSize)
        {
            return Ok( await game.GetSolutions(Response, stage));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserLocalEvent(int roundId, string userName)
        {
            return Ok(await game.GetUserLocalEvent(roundId, userName));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPlayerRoundStatusEvents(int roundId, int userId)
        {
            return Ok(await game.GetPlayerRoundStatusEvents(roundId, userId));
        }
    }
}
