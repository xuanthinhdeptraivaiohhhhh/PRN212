using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class ReadingHistory
{
    public int ReadingId { get; set; }

    public int UserId { get; set; }

    public int NovelId { get; set; }

    public int? ChapterId { get; set; }

    public DateTime? LastReadDate { get; set; }

    public virtual Chapter? Chapter { get; set; }

    public virtual Novel Novel { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
