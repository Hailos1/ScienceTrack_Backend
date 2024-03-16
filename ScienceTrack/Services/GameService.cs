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
        private IConfiguration appConfig;
        public GameService(Repository repository, RandomService random, IConfiguration appConfig)
        {
            this.appConfig = appConfig;
            this.random = random;
            this.repository = repository;
        }

        public async Task<Game> CreateGame()
        {
            var game = new Game();
            game.Status = "created";
            game.Stage = 1;
            game.Date = DateTime.UtcNow;
            game = repository.Games.Create(game);
            await repository.Games.Save();
            return game;
        }

        public async Task<GameUser> AddUser(int gameId, int userId)
        {
            if (repository.GameUsers.GetGameUsers(gameId).Result.Select(x => x.User).Contains(userId))
            {
                return repository.GameUsers.GetGameUsers(gameId).Result.FirstOrDefault(x => x.User == userId);
            }
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
                    Stage = game.Stage,
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
            if (roundUser.LocalSolution == 0)
            {
                
                var gameUser = await repository.GameUsers.context.GameUsers.FirstAsync(x => x.User == userId && x.Game == repository.Rounds.Get(roundUser.Round).Game);
                var ls = repository.LocalSolutions.Get(localSolution);
                gameUser.SocialStatus += ls.SocialStatus;
                gameUser.FinanceStatus += ls.FinanceStatus;
                gameUser.AdministrativeStatus += ls.AdministrativeStatus;
                roundUser.LocalSolution = localSolution;
                await repository.RoundUsers.Save();
            }
            return roundUser;
        }

        public async Task<IEnumerable<LocalSolution>> GetSolutions(HttpResponse response, int stage)
        {
            var solutions = (await repository.LocalSolutions.GetList()).OrderBy(x => x.Id).Skip(1).Where(x => x.Stage == stage || x.Stage == null);
            var count = solutions.Count();
            return solutions;
        }

        public async Task<LocalEventDTO> GetUserLocalEvent(int roundId, string userName)
        {
            var userId = (await repository.Users.Get(userName)).Id;
            return new LocalEventDTO((await repository.RoundUsers.Get(roundId, userId)).LocalEventNavigation);
        }

        public async Task<Round> NextRound(int gameId, int oldRoundId)
        {
            var maxRounds = Convert.ToInt32(appConfig["GameData:countRounds"]);
            var countRounds = repository.Rounds.GetList(gameId).Result.Count();
            if (countRounds < maxRounds && repository.Games.Get(gameId).Status != "finished")
            {
                repository.Games.Get(gameId).Stage = (int)Math.Ceiling(Convert.ToDouble(countRounds) / Convert.ToDouble(appConfig["GameData:stageDuration"]));
                var oldRound = repository.Rounds.Get(oldRoundId);
                oldRound.Status = "finished";
                var globalEvent = random.GetRandomGlobalEvent();
                var round = repository.Rounds.Create(new Round()
                {
                    Game = gameId,
                    GlobalEvent = globalEvent.Id,
                    Stage = (int)Math.Ceiling(Convert.ToDouble(countRounds) / Convert.ToDouble(appConfig["GameData:stageDuration"])),
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
                repository.Rounds.GetList(gameId).Result.Last().Status = "finished";
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
                x.AdministrativeStatus += rd.AdministrativeStatus;
            }
            await repository.RoundUsers.Save();
        }

        public async Task<IEnumerable<PlayerFinalScore>> GetScore(int gameId)
        {
            var gameUsers = (await repository.GameUsers.context.GameUsers.Where(x => x.Game == gameId).ToListAsync()).Select(x => new PlayerFinalScore()
            {
                User = x.User ?? 0,
                UserName = x.User != null ? repository.Users.Get(x.User.Value).UserName : "Удалённый",
                OfficialName = x.User != null ? repository.Users.Get(x.User.Value).OfficialName : "Удалённый",
                Game = gameId,
                AdministrativeStatus = x.AdministrativeStatus.Value,
                SocialStatus = x.SocialStatus.Value,
                FinanceStatus = x.FinanceStatus.Value,
                TotalScore = x.FinanceStatus.Value + x.SocialStatus.Value + x.AdministrativeStatus.Value
            }).OrderByDescending(x => x.TotalScore);
            return gameUsers;
        }
        
        public async Task<IEnumerable<UserTableScoreDto>> GetUserGraph(int gameId, int userId, string? username = null)
        {
            var user = username != null ? await repository.Users.GetQList().FirstOrDefaultAsync(x => x.UserName == username)! : repository.Users.Get(userId);

            var userRounds = await repository.RoundUsers.GetQList().Where(x => x.User == user!.Id && x.RoundNavigation.Game == gameId)
                .Include(x => x.LocalSolutionNavigation)
                .Include(x => x.UserNavigation)
                .Include(x => x.RoundNavigation)
                .ToListAsync();

            var localSolutions = await repository.LocalSolutions.GetQList().ToListAsync();

            var userTable = userRounds.Select(x =>
            {
                Console.WriteLine(x.LocalSolutionNavigation.Stage);
                return new UserTableScoreDto()
                {
                    roundNumber = x.Round,
                    username = x.UserNavigation.UserName,
                    officialName = x.UserNavigation.OfficialName,
                    administrativeUserChoose = x.LocalSolutionNavigation.AdministrativeStatus.Value,
                    socialUserChoose = x.LocalSolutionNavigation.SocialStatus.Value,
                    financeUserChoose = x.LocalSolutionNavigation.FinanceStatus.Value,
                    
                    stage = x.RoundNavigation.Stage != null ? x.RoundNavigation.Stage.Value : 0,

                    adminisrativeMax = localSolutions
                        .Where(l => l.Stage == x.RoundNavigation.Stage || l.Stage == 0)
                        .Max(m => m.AdministrativeStatus.Value),

                    socialMax = localSolutions
                        .Where(l => l.Stage == x.RoundNavigation.Stage || l.Stage == 0)
                        .Max(m => m.SocialStatus.Value),

                    financeMax = localSolutions
                        .Where(l => l.Stage == x.RoundNavigation.Stage || l.Stage == 0)
                        .Max(m => m.FinanceStatus.Value),
                };
            });

            var orderedQueryable = userTable.OrderBy(x => x.roundNumber).ToList();

            for (var i = 1; i <= orderedQueryable.Count(); i++)
            {
                orderedQueryable[i-1].roundNumber = i;
            }
            
            return orderedQueryable;
        }

        public async Task<Game> GetLastGame()
        {
            var game = await repository.Games.GetList();
            return game.Last();
        }

        public async Task<RoundEventsStatus> GetPlayerRoundStatusEvents(int roundId, int userId)
        {
            var RoundUser = repository.RoundUsers.GetList(roundId).Result.FirstOrDefault(x => x.User == userId);
            var Round = repository.Rounds.Get(roundId);
            var GameUser = repository.GameUsers.GetGameUsers(Round.Game).Result.FirstOrDefault(x => x.User == userId);
            var GlobalEvent = repository.GlobalEvents.Get(Round.GlobalEvent);
            var LocalEvent = repository.LocalEvents.Get(RoundUser.LocalEvent);
            var dto = new RoundEventsStatus()
            {
                SocialStatus = GameUser.SocialStatus,
                FinanceStatus = GameUser.FinanceStatus,
                AdministrativeStatus = GameUser.AdministrativeStatus,
                GlobalEvent = new GlobalEventDTO(GlobalEvent),
                LocalEvent = new LocalEventDTO(LocalEvent)
            };
            return dto;
        }
    }
}
