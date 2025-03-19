using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class ManagerAccount
{
    public int ManagerId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? NumberPhone { get; set; }

    public string? Gender { get; set; }

    public bool? Status { get; set; }

    public bool CanLock { get; set; }

    public bool CanApprove { get; set; }

    public string? Role { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<ChapterSubmission> ChapterSubmissions { get; set; } = new List<ChapterSubmission>();

    public virtual ICollection<LockAccountLog> LockAccountLogs { get; set; } = new List<LockAccountLog>();

    public virtual ICollection<LockChapterLog> LockChapterLogs { get; set; } = new List<LockChapterLog>();

    public virtual ICollection<LockNovelLog> LockNovelLogs { get; set; } = new List<LockNovelLog>();

    public virtual ICollection<NovelSubmission> NovelSubmissions { get; set; } = new List<NovelSubmission>();
}
