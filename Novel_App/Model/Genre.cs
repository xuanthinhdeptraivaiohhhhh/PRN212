﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Novel_App.Model;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Novel> Novels { get; set; } = new List<Novel>();
}
