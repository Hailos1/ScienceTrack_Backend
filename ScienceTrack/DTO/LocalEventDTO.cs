using ScienceTrack.Models;

namespace ScienceTrack.DTO
{
    public class LocalEventDTO
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public int? SocialStatus { get; set; }

        public int? FinanceStatus { get; set; }

        public int? AdministrativeStatus { get; set; }

        public decimal? Chance { get; set; }

        public LocalEventDTO(LocalEvent localEvent)
        {
            Id = localEvent.Id;
            Description = localEvent.Description;
            SocialStatus = localEvent.SocialStatus;
            FinanceStatus = localEvent.FinanceStatus;
            AdministrativeStatus = localEvent.AdministrativeStatus;
            Chance = localEvent.Chance;
        }
    }
}
