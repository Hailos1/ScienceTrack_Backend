namespace ScienceTrack.DTO
{
    public class RoundEventsStatus
    {
        public LocalEventDTO LocalEvent { get; set; }
        public GlobalEventDTO GlobalEvent { get; set; }
        public int? SocialStatus { get; set; }

        public int? FinanceStatus { get; set; }

        public int? AdministrativeStatus { get; set; }
    }
}
