using ScienceTrack.Repositories;
using ScienceTrack.DTO;
using ScienceTrack.Models;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace ScienceTrack.Services
{
    public class RoundTimerService
    {
        private Dictionary<int, System.Timers.Timer> realRoundTimers;
        private Dictionary<int, int> startRoundTimers;
        private Dictionary<string, string> UsersConnections = new Dictionary<string, string>();
        public IHubCallerClients? Clients { get; set; }
        
        public RoundTimerService()
        {
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
            realRoundTimers.Add(gameId, new System.Timers.Timer());
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));
            startRoundTimers.Add(gameId, 1);
            realRoundTimers[gameId].Start();           
        }

        private void TickRoundTimer(object obj, ElapsedEventArgs e, int gameId)
        {
            var timer = (System.Timers.Timer)obj;
            startRoundTimers[gameId]++;
            Clients.Clients(UsersConnections.Select(x => x.Value)).SendAsync("CurrentTime", startRoundTimers[gameId]);
            if (startRoundTimers[gameId] >= 10)
            {
                timer.Stop();
                LastTickRoundTimer(gameId);
            }
        }

        private async void LastTickRoundTimer(int obj)
        {
            int gameId = obj;
            var oldRound = new Repository().Rounds.GetList(gameId).Result.Last();
            var newRound = new GameService(new Repository(), new RandomService()).NextRound(gameId, oldRound.Id).Result;
            realRoundTimers[gameId].Stop();
            startRoundTimers.Remove(gameId);
            startRoundTimers.Add(gameId, 1);
            realRoundTimers[gameId] = new System.Timers.Timer(new TimeSpan(50000 * 2));
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));

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
            await Clients.Clients(UsersConnections.Select(x => x.Value)).SendAsync("NewRound", new RoundDTO(newRound));
            realRoundTimers[gameId].Start();
        }
    }
}
