using ScienceTrack.Models;

namespace ScienceTrack.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public int? Stage { get; set; }
        public DateTime? Date { get; set; }
        public int? PlayerCount { get; set; }

        public GameDTO(Game game, int PlayerCount) 
        { 
            Id = game.Id;
            Status = game.Status;
            Stage = game.Stage;
            Date = game.Date;
            this.PlayerCount = PlayerCount;
        }
    }
}
