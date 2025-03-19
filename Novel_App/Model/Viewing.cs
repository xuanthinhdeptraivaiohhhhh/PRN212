using System;
using System.Collections.Generic;

namespace Novel_App.Model;

public partial class Viewing
{
    public int ViewId { get; set; }

    public int NovelId { get; set; }

    public virtual Novel Novel { get; set; } = null!;
}
