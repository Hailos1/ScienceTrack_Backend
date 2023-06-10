using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class LocalSolution
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int? SocialStatus { get; set; }

    public int? FinanceStatus { get; set; }

    public int? AdministrativeStatus { get; set; }
    public int? Stage { get; set; }
    public virtual Stage StageNavigation { get; set; } = null!;

    public virtual ICollection<RoundUser> RoundUsers { get; set; } = new List<RoundUser>();
}
