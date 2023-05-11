﻿using Microsoft.AspNetCore.SignalR;
using ScienceTrack.Repositories;
using ScienceTrack.Services;
using System.Threading;

namespace ScienceTrack.Hubs
{
    public class GameHub : Hub
    {
        private Repository repository;
        private GameService gameService;
        private RoundTimerService timeService;
        public GameHub(Repository repository, GameService gameService) 
        { 
            this.repository = repository;
            this.gameService = gameService;           
            this.timeService = new RoundTimerService(repository, gameService, sendNewRound);
        }

        public async Task StartGame(int gameId)
        {
            var startRound = await gameService.StartGame(gameId);            
            timeService.StartTimer(gameId);
            await Clients.Group(Convert.ToString(gameId)).SendAsync("NewRound", startRound);
        }

        public async Task AddToGroup(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task RemoveFromGroup(string gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        }

        private async void sendNewRound(int gameId, Round round)
        {
            await Clients.Group(Convert.ToString(gameId)).SendAsync("NewRound", round);
        }
    }
}
