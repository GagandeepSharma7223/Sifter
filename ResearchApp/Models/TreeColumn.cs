using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class TreeColumn
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int? ColSeq { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public bool? Display { get; set; }
        public bool? Editable { get; set; }
        public string Fktable { get; set; }
        public string FkjoinCol { get; set; }
        public string FkdisplayCol { get; set; }
        public bool? FkfullList { get; set; }
    }
}
