using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class ChapterSubmission
{
    public int SubmissionCid { get; set; }

    public int ChapterId { get; set; }

    public int UserId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? Status { get; set; }

    public string? ReasonRejected { get; set; }

    public string? Type { get; set; }

    public int? DraftId { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;

    public virtual Novel? Draft { get; set; }

    public virtual ManagerAccount? Manager { get; set; }

    public virtual UserAccount User { get; set; } = null!;
}
