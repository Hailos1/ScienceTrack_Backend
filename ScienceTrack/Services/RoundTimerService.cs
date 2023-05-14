using ScienceTrack.Repositories;
using ScienceTrack.Models;

namespace ScienceTrack.Services
{
    public class RoundTimerService
    {
        private Dictionary<int, Timer> roundTimers;
        private Repository repository;
        private GameService gameService;
        private Action<int, Round> callback;
        public RoundTimerService(Repository repository, GameService gameService, Action<int, Round> callback)
        {
            this.repository = repository;
            this.gameService = gameService;
            roundTimers = new Dictionary<int, Timer>();
            this.callback = callback;
        }

        public void StartTimer(int gameId) 
        {           
            var timerCallback = new TimerCallback(TickRoundTimer);
            Timer timer = new Timer(timerCallback, gameId, 60000 * 2, 60000 * 2);    
            roundTimers.Add(gameId, timer);
        }

        private void TickRoundTimer(object obj)
        {
            int gameId = (int)obj;
            var oldRound = repository.Rounds.GetList(gameId).Result.Last();
            var newRound = gameService.NextRound(gameId, oldRound.Id);
            if (newRound == null) 
            {
                roundTimers[gameId].Dispose();
                roundTimers.Remove(gameId);
                callback(gameId, null);
            }
            if (repository.Rounds.GetList(gameId).Result.Count() == 50)
            {
                return;
            }
            callback(gameId, newRound!.Result);
        }
    }
}
