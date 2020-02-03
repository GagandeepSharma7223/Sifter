using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class MetaCategory
    {
        public int MetaCategoryId { get; set; }
        public int? CategorySeq { get; set; }
        public string Name { get; set; }
    }
}
