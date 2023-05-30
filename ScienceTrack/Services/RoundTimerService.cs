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
        private Repository repository;
        private GameService gameService;
        private Action<int, Round, IHubCallerClients> roundCallback = sendNewRound;
        private Action<int, int, IHubCallerClients> timeCallback = sendCurrentTime;
        public IHubCallerClients Clients { get; set; }
        
        public RoundTimerService(Repository repository, GameService gameService)
        {
            this.repository = repository;
            this.gameService = gameService;
            realRoundTimers = new Dictionary<int, System.Timers.Timer>();
            startRoundTimers = new Dictionary<int, int>();
        }

        public void StartTimer(int gameId) 
        {
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
            timeCallback(gameId, startRoundTimers[gameId], Clients);
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
                roundCallback(gameId, null, Clients);
            }
            if (repository.Rounds.GetList(gameId).Result.Count() == 50)
            {
                return;
            }
            roundCallback(gameId, newRound!.Result, Clients);
            realRoundTimers[gameId].Start();
        }
        private static async void sendNewRound(int gameId, Round round, IHubCallerClients clients)
        {
            var rep = new Repository();
            await clients.Group(Convert.ToString(gameId)).SendAsync("NewRound", round);
        }

        private static async void sendCurrentTime(int gameId, int time, IHubCallerClients clients)
        {
            await clients.Group(Convert.ToString(gameId)).SendAsync("CurrentTime", time);
        }
    }
}
