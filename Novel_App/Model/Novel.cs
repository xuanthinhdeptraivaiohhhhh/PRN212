using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class Novel
{
    public int NovelId { get; set; }

    public string NovelName { get; set; } = null!;

    public int UserId { get; set; }

    public string? ImageUrl { get; set; }

    public string? NovelDescription { get; set; }

    public int? TotalChapter { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? NovelStatus { get; set; }

    public virtual ICollection<ChapterSubmission> ChapterSubmissions { get; set; } = new List<ChapterSubmission>();

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<LockNovelLog> LockNovelLogs { get; set; } = new List<LockNovelLog>();

    public virtual ICollection<NovelSubmission> NovelSubmissionDrafts { get; set; } = new List<NovelSubmission>();

    public virtual ICollection<NovelSubmission> NovelSubmissionNovels { get; set; } = new List<NovelSubmission>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<ReadingHistory> ReadingHistories { get; set; } = new List<ReadingHistory>();

    public virtual UserAccount User { get; set; } = null!;

    public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
