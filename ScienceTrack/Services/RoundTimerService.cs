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
        private Dictionary<int, Dictionary<string, string>> UsersConnections = new Dictionary<int, Dictionary<string, string>>();
        public IHubCallerClients? Clients { get; set; }
        
        public RoundTimerService()
        {
            realRoundTimers = new Dictionary<int, System.Timers.Timer>();
            startRoundTimers = new Dictionary<int, int>();
        }

        public void ChangeUserConnection(string userName, string connectionId, int gameId)
        {
            if (!UsersConnections.ContainsKey(gameId))
            {
                UsersConnections.Add(gameId, new Dictionary<string, string>());
            }
            if (UsersConnections[gameId].ContainsKey(userName))
            {
                UsersConnections[gameId][userName] = connectionId;
            }
            UsersConnections[gameId].Add(userName, connectionId);
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
            startRoundTimers.Add(gameId, 0);
            realRoundTimers[gameId].Start();           
        }

        private void TickRoundTimer(object obj, ElapsedEventArgs e, int gameId)
        {
            var timer = (System.Timers.Timer)obj;
            startRoundTimers[gameId]++;
            Clients.Clients(UsersConnections[gameId].Select(x => x.Value)).SendAsync("CurrentTime", startRoundTimers[gameId]);
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
            startRoundTimers.Add(gameId, 0);
            realRoundTimers[gameId] = new System.Timers.Timer(50000 * 2);
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));

            if (newRound == null) 
            {
                realRoundTimers[gameId].Dispose();
                realRoundTimers.Remove(gameId);
                startRoundTimers.Remove(gameId);
                UsersConnections[gameId].Select(x => Clients.Client(x.Value).SendAsync("NewRound", null));
            }
            if (new Repository().Rounds.GetList(gameId).Result.Count() == 48)
            {
                return;
            }
            var dto = new RoundDTO(newRound);
            var stage = new Repository().Stages.Get(new Repository().Games.Get(gameId).Stage.Value);
            dto.Stage = stage.Id;
            dto.StageDisc = stage.Desc;
            dto.Picture = stage.PicturePath;
            await Clients.Clients(UsersConnections[gameId].Select(x => x.Value)).SendAsync("NewRound", dto);
            realRoundTimers[gameId].Start();
        }
    }
}
