using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScienceTrack.Models;
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
            timeService.StartTimer(gameId);
            await Clients.Group(Convert.ToString(gameId)).SendAsync("NewRound", startRound);
        }
        [Authorize]
        public async Task AddToGroup(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
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
