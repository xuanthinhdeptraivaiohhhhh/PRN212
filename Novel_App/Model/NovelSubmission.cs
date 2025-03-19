using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class NovelSubmission
{
    public int SubmissionNid { get; set; }

    public int NovelId { get; set; }

    public int UserId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? Status { get; set; }

    public string? ReasonRejected { get; set; }

    public string? Type { get; set; }

    public int? DraftId { get; set; }

    public virtual Novel? Draft { get; set; }

    public virtual ManagerAccount? Manager { get; set; }

    public virtual Novel Novel { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
