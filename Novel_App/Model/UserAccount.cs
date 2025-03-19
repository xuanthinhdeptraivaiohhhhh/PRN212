using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? ImageUml { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? NumberPhone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public bool? Status { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<ChapterSubmission> ChapterSubmissions { get; set; } = new List<ChapterSubmission>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<LockAccountLog> LockAccountLogs { get; set; } = new List<LockAccountLog>();

    public virtual ICollection<NovelSubmission> NovelSubmissions { get; set; } = new List<NovelSubmission>();

    public virtual ICollection<Novel> Novels { get; set; } = new List<Novel>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
}
