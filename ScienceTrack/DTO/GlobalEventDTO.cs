using ScienceTrack.Models;

namespace ScienceTrack.DTO
{
    public class GlobalEventDTO
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public int? SocialStatus { get; set; }

        public int? FinanceStatus { get; set; }

        public int? AdministrativeStatus { get; set; }

        public decimal? Chance { get; set; }

        public GlobalEventDTO(GlobalEvent globalEvent)
        {
            Id = globalEvent.Id;
            Description = globalEvent.Description;
            SocialStatus = globalEvent.SocialStatus;
            FinanceStatus = globalEvent.FinanceStatus;
            AdministrativeStatus = globalEvent.AdministrativeStatus;
            Chance = globalEvent.Chance;
        }
    }
}
