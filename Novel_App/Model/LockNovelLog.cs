using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class LockNovelLog
{
    public int LogNid { get; set; }

    public int ManagerId { get; set; }

    public int NovelId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Action { get; set; }

    public string? LockReason { get; set; }

    public virtual ManagerAccount Manager { get; set; } = null!;

    public virtual Novel Novel { get; set; } = null!;
}
