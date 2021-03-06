﻿using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Unit
    {
        public int? UnitID { get; set; }
        public int? WorkID { get; set; }
        public int? StartPage { get; set; }
        public int? CategoryID { get; set; }
        public string LiteralTitle { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string TitleEnglish { get; set; }
        public string Attribution { get; set; }

        public virtual Work Work { get; set; }
        public virtual Category Category { get; set; }
    }
}
