using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class Rating
{
    public int RatingId { get; set; }

    public int UserId { get; set; }

    public int NovelId { get; set; }

    public double? Score { get; set; }

    public virtual Novel Novel { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
