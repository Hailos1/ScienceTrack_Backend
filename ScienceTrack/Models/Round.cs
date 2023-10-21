using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class Round
{
    public int Id { get; set; }
    public int Game { get; set; }
    public string? Status { get; set; }
    public int GlobalEvent { get; set; }
    public virtual Game GameNavigation { get; set; } = null!;
    public virtual GlobalEvent GlobalEventNavigation { get; set; } = null!;
    public virtual ICollection<RoundUser> RoundUsers { get; set; } = new List<RoundUser>();
}
