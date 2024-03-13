using System;
using System.Collections.Generic;

namespace ScienceTrack.Models;

public partial class GameUser
{
    public int Id { get; set; }

    public int Game { get; set; }

    public int? User { get; set; }

    public int? SocialStatus { get; set; }

    public int? FinanceStatus { get; set; }

    public int? AdministrativeStatus { get; set; }

    public virtual Game GameNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
