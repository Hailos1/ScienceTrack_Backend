using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ScienceTrack.Models;

namespace ScienceTrack;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();
    [JsonIgnore]
    public virtual ICollection<RoundUser> RoundUsers { get; set; } = new List<RoundUser>();
}
