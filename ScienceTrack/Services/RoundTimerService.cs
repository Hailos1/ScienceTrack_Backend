using ScienceTrack.Repositories;
using ScienceTrack.DTO;
using ScienceTrack.Models;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace ScienceTrack.Services
{
    public class RoundTimerService
    {
        private Dictionary<int, System.Timers.Timer> realRoundTimers;
        private Dictionary<int, int> startRoundTimers;
        private Dictionary<int, int> gameCurrentRoundTime = new Dictionary<int, int>();
        private Dictionary<int, Dictionary<string, string>> UsersConnections = new Dictionary<int, Dictionary<string, string>>();
        public IHubCallerClients? Clients { get; set; }
        private IConfiguration appConfig { get; set; }
        
        public RoundTimerService(IConfiguration appConfig)
        {
            realRoundTimers = new Dictionary<int, System.Timers.Timer>();
            startRoundTimers = new Dictionary<int, int>();
            this.appConfig = appConfig;
        }

        public async Task ChangeUserConnection(string userName, string connectionId, int gameId)
        {
            if (!UsersConnections.ContainsKey(gameId))
            {
                UsersConnections.Add(gameId, new Dictionary<string, string>());
            }
            UsersConnections[gameId][userName] = connectionId;
        }

        public async Task StartTimer(int gameId) 
        {
            if (realRoundTimers.ContainsKey(gameId))
            {
                return;
            }
            realRoundTimers.Add(gameId, new System.Timers.Timer());
            realRoundTimers[gameId].Interval = 1000;
            realRoundTimers[gameId].Elapsed += new ElapsedEventHandler((sender, args) => TickRoundTimer(sender, args, gameId));
            startRoundTimers.Add(gameId, 0);
            gameCurrentRoundTime[gameId] = new Repository().Games.GetQList().Include(x => x.StageNavigation).First(x => x.Id == gameId).StageNavigation.RoundDuration;
            realRoundTimers[gameId].Start();           
        }

        private async Task TickRoundTimer(object obj, ElapsedEventArgs e, int gameId)
        {
            var timer = (System.Timers.Timer)obj;
            startRoundTimers[gameId]++;
            await Clients.Clients(UsersConnections[gameId].Select(x => x.Value)).SendAsync("CurrentTime", startRoundTimers[gameId]);
            var duration = gameCurrentRoundTime[gameId];
            if (duration == 0) 
            {
                duration = 104;
            }

            if (startRoundTimers[gameId] >= duration)
            {
                timer.Stop();
                LastTickRoundTimer(gameId);
            }
        }

        private async Task LastTickRoundTimer(int obj)
        {
            int gameId = obj;
            var oldRound = new Repository().Rounds.GetList(gameId).Result.Last();
            var newRound = new GameService(new Repository(), new RandomService(), appConfig).NextRound(gameId, oldRound.Id).Result;
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
                await Clients.Clients(UsersConnections[gameId].Select(x => x.Value)).SendAsync("NewRound", "end");
                return;
            }

            gameCurrentRoundTime[gameId] = new Repository().Games.GetQList().Include(x => x.StageNavigation).First(x => x.Id == gameId).StageNavigation.RoundDuration;

            if (new Repository().Rounds.GetList(gameId).Result.Count() == Convert.ToInt32(appConfig["GameData:countRounds"]))
            {
                //return;
            }

            var dto = new RoundDTO(newRound);
            dto.RoundDuration = gameCurrentRoundTime[gameId] == 0 ? 104 : gameCurrentRoundTime[gameId];
            var stage = new Repository().Stages.Get(new Repository().Games.Get(gameId).Stage.Value);
            dto.Stage = stage.Id;
            dto.StageDisc = stage.Desc;
            dto.Picture = stage.PicturePath;
            await Clients.Clients(UsersConnections[gameId].Select(x => x.Value)).SendAsync("NewRound", dto);
            realRoundTimers[gameId].Start();
        }
    }
}
