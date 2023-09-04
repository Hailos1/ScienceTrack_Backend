using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class Game
{
    public int Id { get; set; }

    public string? Status { get; set; }
    public int? Stage { get; set; }
    public DateTime? Date { get; set; }
    public virtual Stage StageNavigation { get; set; } = null!;

    public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
