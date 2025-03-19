using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class LockChapterLog
{
    public int LogCid { get; set; }

    public int ManagerId { get; set; }

    public int ChapterId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Action { get; set; }

    public string? LockReason { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;

    public virtual ManagerAccount Manager { get; set; } = null!;
}
