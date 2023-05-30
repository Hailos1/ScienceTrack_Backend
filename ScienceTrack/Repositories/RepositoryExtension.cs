using Microsoft.EntityFrameworkCore;
using ScienceTrack.Models;
using System.Runtime.CompilerServices;

namespace ScienceTrack.Repositories
{
    public static class RepositoryExtension
    {
        public static async Task<IEnumerable<GameUser>> GetGameUsers(this GenericRepository<GameUser> repository, int gameId)
        {
            return await repository.context.GameUsers.Where(x => x.Game == gameId).ToListAsync();
        }

        public static async Task<RoundUser> Get(this GenericRepository<RoundUser> repository, int roundId, int userId)
        {
            return await repository.context.RoundUsers.FirstAsync(x => x.Round == roundId && x.User == userId);
        }

        public static async Task<IEnumerable<RoundUser>> GetList(this GenericRepository<RoundUser> repository, int roundId)
        {
            return await repository.context.RoundUsers.Where(x => x.Round == roundId).ToListAsync();
        }

        public static async Task<IEnumerable<Round>> GetList(this GenericRepository<Round> repository, int gameId)
        {
            return await repository.context.Rounds.Where(x => x.Game == gameId).ToListAsync();
        }

        public static async Task<User> Get(this GenericRepository<User> repository, string userName)
        {
            return await repository.context.Users.FirstAsync(x => x.UserName == userName);
        }
    }
}
