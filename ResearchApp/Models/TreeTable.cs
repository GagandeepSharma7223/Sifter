using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class TreeTable
    {
        public int? CategorySeq { get; set; }
        public int? TableSeq { get; set; }
        public string TableName { get; set; }
        public string DisplayName { get; set; }
        public bool? Editable { get; set; }
    }
}
