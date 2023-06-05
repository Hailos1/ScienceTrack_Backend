namespace ScienceTrack.Models
{
    public class Stage
    {
        public int Id { get; set; }

        public string? Desc { get; set; }
        public string? PicturePath { get; set; }
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
