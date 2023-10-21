using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ScienceTrack.Models;

public partial class GlobalEvent
{
    public int Id { get; set; }
    
    public string Description { get; set; } = null!;

    public int? SocialStatus { get; set; }

    public int? FinanceStatus { get; set; }

    public int? AdministrativeStatus { get; set; }

    public decimal? Chance { get; set; }

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
