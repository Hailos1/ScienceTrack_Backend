using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class RoundUser
{
    public int Id { get; set; }

    public int Round { get; set; }

    public int? User { get; set; }

    public int? LocalSolution { get; set; }

    public int LocalEvent { get; set; }

    public virtual LocalEvent LocalEventNavigation { get; set; } = null!;

    public virtual LocalSolution? LocalSolutionNavigation { get; set; }

    public virtual Round RoundNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
