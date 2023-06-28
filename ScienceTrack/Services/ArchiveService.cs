using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.Services
{
    public class ArchiveService
    {
        private Repository repository;
        private IConfiguration appConfig;
        public ArchiveService(Repository repository, IConfiguration appConfig)
        {
            this.appConfig = appConfig;
            this.repository = repository;
        }

        public async Task<IEnumerable<Game>> GetActiveGames(HttpResponse response, int pageNum = 1, int pageSize = 10)
        {
            var games = (await repository.Games.GetList()).Where(x => x.Status == "started").OrderByDescending(x => x.Date);
            var count = games.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            response.Headers.Add("TotalPages", $"{totalPages}");
            return games
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IEnumerable<Game>> GetPendingGames(HttpResponse response, int pageNum = 1, int pageSize = 10)
        {
            var games = (await repository.Games.GetList()).Where(x => x.Status == "created").OrderByDescending(x => x.Date);
            var count = games.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            response.Headers.Add("TotalPages", $"{totalPages}");
            return games
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IEnumerable<Game>> GetArchivedGames(HttpResponse response, int pageNum = 1, int pageSize = 10)
        {
            var games = (await repository.Games.GetList()).Where(x => x.Status == "finished").OrderByDescending(x => x.Date);
            var count = games.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            response.Headers.Add("TotalPages", $"{totalPages}");
            return games
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
