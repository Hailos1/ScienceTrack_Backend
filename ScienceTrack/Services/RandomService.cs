using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.Services
{
    public class RandomService
    {
        private Dictionary<int, decimal> globalEvents;
        private Dictionary<int, decimal> localEvents;
        private List<int> globalEventsSoup = new List<int>();
        private List<int> localEventsSoup = new List<int>();
        private Repository repository;

        public RandomService()
        {
            this.repository = new Repository();
            foreach(var obj in repository.GlobalEvents.GetList().Result)
            {
                globalEventsSoup.AddRange(Enumerable.Range(0, Convert.ToInt32(obj.Chance * 1000)).Select(x => obj.Id));
            }
            foreach (var obj in repository.LocalEvents.GetList().Result)
            {
                localEventsSoup.AddRange(Enumerable.Range(0, Convert.ToInt32(obj.Chance * 1000)).Select(x => obj.Id));
            }
        }

        public GlobalEvent GetRandomGlobalEvent()
        {
            var random = new Random();
            return repository.GlobalEvents.Get(globalEventsSoup[random.Next(globalEventsSoup.Count - 1)]);
        }

        public LocalEvent GetRandomLocalEvent()
        {
            var random = new Random();
            return repository.LocalEvents.Get(localEventsSoup[random.Next(localEventsSoup.Count - 1)]);
        }
    }
}
