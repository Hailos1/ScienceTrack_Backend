using System.Text.Json.Serialization;

namespace ScienceTrack.Models
{
    public class Stage
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int RoundDuration { get; set; }
        public string? Desc { get; set; }
        public string? PicturePath { get; set; }
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
        public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
        public virtual ICollection<LocalSolution> LocalSolutions { get; set; } = new List<LocalSolution>();
    }
}
