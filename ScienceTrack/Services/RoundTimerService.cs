using ScienceTrack.Repositories;
using ScienceTrack.Models;
using System.Timers;
using Microsoft.AspNetCore.SignalR;

namespace ScienceTrack.Services
{
    public class RoundTimerService
    {
        private Dictionary<int, System.Timers.Timer> realRoundTimers;
        private Dictionary<int, int> startRoundTimers;
        private Dictionary<int, IHubCallerClients> GameClients;
        private Repository repository;
        private GameService gameService;
        
        public RoundTimerService(Repository repository, GameService gameService)
        {
            this.repository = repository;
            this.gameService = gameService;
            realRoundTimers = new Dictionary<int, System.Timers.Timer>();
            startRoundTimers = new Dictionary<int, int>();
        }

        public void StartTimer(int gameId, IHubCallerClients clients) 
        {
            GameClients.Add(gameId, clients);
            realRoundTimers.Add(gameId, new System.Timers.Timer(new TimeSpan(60000 * 2)));
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));
            startRoundTimers.Add(gameId, 0);
            realRoundTimers[gameId].Start();           
        }

        private void TickRoundTimer(object obj, ElapsedEventArgs e, int gameId)
        {
            var timer = (System.Timers.Timer)obj;
            startRoundTimers[gameId]++;
            sendCurrentTime(gameId, startRoundTimers[gameId]);
            if (startRoundTimers[gameId] >= 120)
            {
                timer.Stop();
                LastTickRoundTimer(gameId);
            }
        }

        private void LastTickRoundTimer(int obj)
        {
            int gameId = obj;
            var oldRound = repository.Rounds.GetList(gameId).Result.Last();
            var newRound = gameService.NextRound(gameId, oldRound.Id);
            realRoundTimers[gameId].Stop();
            startRoundTimers.Remove(gameId);
            startRoundTimers.Add(gameId, 0);
            realRoundTimers[gameId] = new System.Timers.Timer(new TimeSpan(60000 * 2));
            if (newRound == null) 
            {
                realRoundTimers[gameId].Dispose();
                realRoundTimers.Remove(gameId);
                startRoundTimers.Remove(gameId);
                sendNewRound(gameId, null);
            }
            if (repository.Rounds.GetList(gameId).Result.Count() == 50)
            {
                return;
            }
            sendNewRound(gameId, newRound!.Result);
            realRoundTimers[gameId].Start();
        }

        private async void sendNewRound(int gameId, Round round)
        {
            await GameClients[gameId].Group(Convert.ToString(gameId)).SendAsync("NewRound", round, gameService.GetRoundUsers(round.Id));
        }

        private async void sendCurrentTime(int gameId, int time)
        {
            await GameClients[gameId].Group(Convert.ToString(gameId)).SendAsync("CurrentTime", time);
        }
    }
}
