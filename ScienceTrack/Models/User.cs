using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;
    public string OfficialName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? Role { get; set; }

    public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();

    public virtual Role? RoleNavigation { get; set; }

    public virtual ICollection<RoundUser> RoundUsers { get; set; } = new List<RoundUser>();
}
