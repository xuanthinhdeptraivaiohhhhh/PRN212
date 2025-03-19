using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class Comment
{
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int NovelId { get; set; }

    public string CommentContent { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public virtual Novel Novel { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
