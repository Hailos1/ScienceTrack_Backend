using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.DTO
{
    public class RoundDTO
    {
        public int Id { get; set; }

        public int Game { get; set; }
        public int RoundDuration { get; set; }
        public string? Status { get; set; }

        public int GlobalEvent { get; set; }
        public int Age { get; set; }
        public int? Stage { get; set; }
        public string? StageDisc { get; set; }
        public string? Picture { get; set; } 

        public RoundDTO(Round round)
        {
            Id = round.Id;
            Game = round.Game;
            Status = round.Status;
            GlobalEvent = round.GlobalEvent;
            Age = new Repository().Rounds.GetList(Game).Result.Count() * 2 + 20;
        }
    }
}
