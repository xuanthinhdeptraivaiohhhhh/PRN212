using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class Chapter
{
    public int ChapterId { get; set; }

    public int NovelId { get; set; }

    public int ChapterNumber { get; set; }

    public string? ChapterName { get; set; }

    public string? FileUrl { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? ChapterStatus { get; set; }

    public virtual ICollection<ChapterSubmission> ChapterSubmissions { get; set; } = new List<ChapterSubmission>();

    public virtual ICollection<LockChapterLog> LockChapterLogs { get; set; } = new List<LockChapterLog>();

    public virtual Novel Novel { get; set; } = null!;

    public virtual ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();
}
