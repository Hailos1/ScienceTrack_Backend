﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScienceTrack.DTO;
using ScienceTrack.Repositories;
using ScienceTrack.Services;

namespace ScienceTrack.Hubs
{
    public class GameHub : Hub
    {
        private Repository repository;
        private GameService gameService;
        private RoundTimerService timeService;
        public GameHub(Repository repository, GameService gameService, RoundTimerService roundTimerService)
        {
            this.repository = repository;
            this.gameService = gameService;
            this.timeService = roundTimerService;
        }
        [Authorize(Roles = "admin")]
        public async Task StartGame(int gameId)
        {
            timeService.Clients = Clients;
            var startRound = await gameService.StartGame(gameId);
            await timeService.StartTimer(gameId);
            var dto = new RoundDTO(startRound);
            var stage = repository.Stages.Get(repository.Games.Get(startRound.Game).Stage.Value);
            dto.Stage = stage.Id;
            dto.StageDisc = stage.Desc;
            dto.Picture = stage.PicturePath;
            dto.RoundDuration = stage.RoundDuration;
            await Clients.Group(Convert.ToString(gameId)).SendAsync("NewRound", dto);
        }
        [Authorize]
        public async Task AddToGroup(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            timeService.ChangeUserConnection(Context.UserIdentifier, Context.ConnectionId, Convert.ToInt32(gameId));
            timeService.Clients = Clients;
        }
        [Authorize]
        public async Task RemoveFromGroup(string gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
            timeService.Clients = Clients;
        }
    }
}
