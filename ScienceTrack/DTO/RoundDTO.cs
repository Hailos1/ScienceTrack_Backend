using ScienceTrack.Models;

namespace ScienceTrack.DTO
{
    public class RoundDTO
    {
        public int Id { get; set; }

        public int Game { get; set; }

        public string? Status { get; set; }

        public int GlobalEvent { get; set; }

        public RoundDTO(Round round)
        {
            Id = round.Id;
            Game = round.Game;
            Status = round.Status;
            GlobalEvent = round.GlobalEvent;
        }
    }
}
