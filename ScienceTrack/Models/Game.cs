using Microsoft.OpenApi.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ScienceTrack.Models;

public partial class Game
{
    [Display("gameId")]
    public int Id { get; set; }

    public string? Status { get; set; }

    [JsonIgnore]
    public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();
    [JsonIgnore]
    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
