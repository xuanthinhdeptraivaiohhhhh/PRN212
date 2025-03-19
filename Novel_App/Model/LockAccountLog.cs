using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class LockAccountLog
{
    public int LogAid { get; set; }

    public int ManagerId { get; set; }

    public int UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Action { get; set; }

    public string? LockReason { get; set; }

    public virtual ManagerAccount Manager { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
