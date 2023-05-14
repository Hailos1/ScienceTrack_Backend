using Microsoft.EntityFrameworkCore;
using ScienceTrack.DTO;
using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.Services
{
    public class GameService
    {
        private Repository repository;
        private RandomService random;
        public GameService(Repository repository, RandomService random)
        {
            this.random = random;
            this.repository = repository;
        }

        public async Task<Game> CreateGame()
        {
            var game = new Game();
            game.Status = "created";
            game = repository.Games.Create(game);
            await repository.Games.Save();
            return game;
        }

        public async Task<GameUser> AddUser(int gameId, int userId)
        {
            if (repository.Games.Get(gameId).Status == "created")
            {
                var gameUser = new GameUser()
                {
                    AdministrativeStatus = 0,
                    FinanceStatus = 0,
                    SocialStatus = 0,
                    Game = gameId,
                    User = userId
                };
                var user = repository.GameUsers.Create(gameUser);
                await repository.GameUsers.Save();
                return user;
            }
            else
                return null;
            }

            public async Task<Round> StartGame(int gameId)
        {
            var game = repository.Games.Get(gameId);
            if (game.Status == "created")
            {
                game.Status = "started";
                var round = repository.Rounds.Create(new Round()
                {
                    Game = gameId,
                    GlobalEvent = random.GetRandomGlobalEvent().Id,
                    Status = game.Status
                });
                await repository.Rounds.Save();
                await InitialRoundUsers(gameId, Convert.ToInt32(round.Id));
                return round;
            }
            else
                return null;
        }

        public async Task<RoundUser> PlayerChoose(int roundId, int userId, int localSolution)
        {
            var roundUser = await repository.RoundUsers.context.RoundUsers.FirstAsync(x => x.Round == roundId && x.User == userId);
            var gameUser = await repository.GameUsers.context.GameUsers.FirstAsync(x => x.User == userId);
            var ls = repository.LocalSolutions.Get(localSolution);
            gameUser.SocialStatus += ls.SocialStatus;
            gameUser.FinanceStatus += ls.FinanceStatus;
            gameUser.AdministrativeStatus += ls.AdministrativeStatus;
            roundUser.LocalSolution = localSolution;
            await repository.RoundUsers.Save();
            return roundUser;
        }

        public async Task<Round> NextRound(int gameId, int oldRoundId)
        {
            if (repository.Rounds.GetList(gameId).Result.Count() < 50 && repository.Games.Get(gameId).Status != "finished")
            {
                var oldRound = repository.Rounds.Get(oldRoundId);
                oldRound.Status = "finished";
                var globalEvent = random.GetRandomGlobalEvent();
                var round = repository.Rounds.Create(new Round()
                {
                    Game = gameId,
                    GlobalEvent = globalEvent.Id,
                    Status = "started"
                });
                repository.GameUsers.GetGameUsers(gameId).Result.AsParallel().ForAll(x =>
                {
                    x.SocialStatus += globalEvent.SocialStatus;
                    x.FinanceStatus += globalEvent.FinanceStatus;
                    x.AdministrativeStatus += globalEvent.AdministrativeStatus;
                });
                await repository.Rounds.Save();
                await InitialRoundUsers(gameId, Convert.ToInt32(round.Id));
                return round;
            }
            else
            {
                repository.Games.Get(gameId).Status = "finished";
                await repository.Games.Save();
                return null;
            }
        }

        public async Task<IEnumerable<RoundUser>> GetRoundUsers(int roundId)
        {
            return await repository.RoundUsers.context.RoundUsers.Where(x => x.Round == roundId).ToListAsync();
        }

        public async Task<bool> FinishGame(int gameId)
        {
            repository.Games.Get(gameId).Status = "finished";
            await repository.Games.Save();
            return true;
        }

        private async Task InitialRoundUsers(int gameId, int roundId)
        {
            var gameUsers = repository.GameUsers.context.GameUsers.Where(x => x.Game == gameId);
            foreach (var x in gameUsers)
            {
                var rd = random.GetRandomLocalEvent();
                repository.RoundUsers.Create(new RoundUser()
                {
                    LocalSolution = 0,
                    LocalEvent = rd.Id,
                    Round = roundId,
                    User = x.User
                });
                x.SocialStatus += rd.SocialStatus;
                x.FinanceStatus += rd.FinanceStatus;
                x.AdministrativeStatus += x.AdministrativeStatus;
            }
            await repository.RoundUsers.Save();
        }

        public async Task<IEnumerable<PlayerFinalScore>> GetScore(int gameId)
        {
            var gameUsers = (await repository.GameUsers.context.GameUsers.Where(x => x.Game == gameId).ToListAsync()).Select(x => new PlayerFinalScore()
            {
                User = x.User,
                UserName = repository.Users.Get(x.User).UserName,
                Game = gameId,
                AdministrativeStatus = x.AdministrativeStatus.Value,
                SocialStatus = x.SocialStatus.Value,
                FinanceStatus = x.FinanceStatus.Value
            });
            return gameUsers;
        }
    }
}
