using Microsoft.EntityFrameworkCore;
using ScienceTrack.Models;

namespace ScienceTrack.Repositories
{
    public class Repository
    {
        private ScienceTrackContext context = new ScienceTrackContext();
        private GenericRepository<Game> games { get; set; }

        private GenericRepository<GameUser> gameUsers { get; set; }

        private GenericRepository<GlobalEvent> globalEvents { get; set; }

        private GenericRepository<LocalEvent> localEvents { get; set; }

        private GenericRepository<LocalSolution> localSolutions { get; set; }

        private GenericRepository<Round> rounds { get; set; }

        private GenericRepository<RoundUser> roundUsers { get; set; }

        private GenericRepository<User> users { get; set; }

        public GenericRepository<Game> Games
        {
            get
            {
                if (this.games == null)
                {
                    this.games = new GenericRepository<Game>(context);
                }
                return games;
            }
        }

        public GenericRepository<GameUser> GameUsers
        {
            get
            {
                if (this.gameUsers == null)
                {
                    this.gameUsers = new GenericRepository<GameUser>(context);
                }
                return gameUsers;
            }
        }

        public GenericRepository<GlobalEvent> GlobalEvents
        {
            get
            {
                if (this.globalEvents == null)
                {
                    this.globalEvents = new GenericRepository<GlobalEvent>(context);
                }
                return globalEvents;
            }
        }

        public GenericRepository<LocalEvent> LocalEvents
        {
            get
            {
                if (this.localEvents == null)
                {
                    this.localEvents = new GenericRepository<LocalEvent>(context);
                }
                return localEvents;
            }
        }

        public GenericRepository<LocalSolution> LocalSolutions
        {
            get
            {
                if (this.localSolutions == null)
                {
                    this.localSolutions = new GenericRepository<LocalSolution>(context);
                }
                return localSolutions;
            }
        }

        public GenericRepository<Round> Rounds
        {
            get
            {
                if (this.rounds == null)
                {
                    this.rounds = new GenericRepository<Round>(context);
                }
                return rounds;
            }
        }

        public GenericRepository<RoundUser> RoundUsers
        {
            get
            {
                if (this.roundUsers == null)
                {
                    this.roundUsers = new GenericRepository<RoundUser>(context);
                }
                return roundUsers;
            }
        }

        public GenericRepository<User> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new GenericRepository<User>(context);
                }
                return users;
            }
        }
    }
}
