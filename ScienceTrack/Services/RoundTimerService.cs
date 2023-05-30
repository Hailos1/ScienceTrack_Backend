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
        //private Repository repository = new Repository();
        //private RandomService randomService = new RandomService();
        //private GameService gameService;
        //private Action<int, Round, IHubCallerClients, Dictionary<string, string>> roundCallback = sendNewRound;
        private Action<int, int, IHubCallerClients, Dictionary<string, string>> timeCallback = sendCurrentTime;
        private Dictionary<string, string> UsersConnections = new Dictionary<string, string>();
        public IHubCallerClients? Clients { get; set; }
        
        public RoundTimerService()
        {
            //gameService = new GameService(repository, randomService);
            realRoundTimers = new Dictionary<int, System.Timers.Timer>();
            startRoundTimers = new Dictionary<int, int>();
        }

        public void ChangeUserConnection(string userName, string connectionId)
        {
            if (UsersConnections.ContainsKey(userName))
            {
                UsersConnections[userName] = connectionId;
            }
            UsersConnections.Add(userName, connectionId);
        }

        public void StartTimer(int gameId) 
        {
            if (realRoundTimers.ContainsKey(gameId))
            {
                return;
            }
            realRoundTimers.Add(gameId, new System.Timers.Timer(new TimeSpan(50000 * 2)));
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));
            startRoundTimers.Add(gameId, 1);
            realRoundTimers[gameId].Start();           
        }

        private void TickRoundTimer(object obj, ElapsedEventArgs e, int gameId)
        {
            var timer = (System.Timers.Timer)obj;
            startRoundTimers[gameId]++;
            timeCallback(gameId, startRoundTimers[gameId], Clients, UsersConnections);
            if (startRoundTimers[gameId] >= 100)
            {
                timer.Stop();
                LastTickRoundTimer(gameId);
            }
        }

        private void LastTickRoundTimer(int obj)
        {
            int gameId = obj;
            var oldRound = new Repository().Rounds.GetList(gameId).Result.Last();
            var newRound = new GameService(new Repository(), new RandomService()).NextRound(gameId, oldRound.Id);
            realRoundTimers[gameId].Stop();
            startRoundTimers.Remove(gameId);
            startRoundTimers.Add(gameId, 1);
            realRoundTimers[gameId] = new System.Timers.Timer(new TimeSpan(60000 * 2));
            if (newRound == null) 
            {
                realRoundTimers[gameId].Dispose();
                realRoundTimers.Remove(gameId);
                startRoundTimers.Remove(gameId);
                UsersConnections.Select(x => Clients.Client(x.Value).SendAsync("NewRound", null));
            }
            if (new Repository().Rounds.GetList(gameId).Result.Count() == 50)
            {
                return;
            }
            UsersConnections.Select(x => Clients.Client(x.Value).SendAsync("NewRound", newRound));
            realRoundTimers[gameId].Start();
        }

        private static async void sendCurrentTime(int gameId, int time, IHubCallerClients clients, Dictionary<string, string> UsersConnections)
        {
            //await clients.Clients(UsersConnections.Select(x => x.Value)).SendAsync("CurrentTime", time);
            await clients.Group(Convert.ToString(gameId)).SendAsync("CurrentTime", time);
        }
    }
}
