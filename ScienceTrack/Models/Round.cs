using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Attributes;
using ScienceTrack.Models;

namespace ScienceTrack;

public partial class Round
{
    [Display("roundId")]
    public int Id { get; set; }

    public int Game { get; set; }

    public string? Status { get; set; }

    public int GlobalEvent { get; set; }
    public virtual Game GameNavigation { get; set; } = null!;
    public virtual GlobalEvent GlobalEventNavigation { get; set; } = null!;
    public virtual ICollection<RoundUser> RoundUsers { get; set; } = new List<RoundUser>();
}
