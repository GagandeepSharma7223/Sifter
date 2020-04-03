using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class MetaColumn
    {
        public int? MetaColumnId { get; set; }
        public string TableName { get; set; }
        public int? ColSeq { get; set; }
        public string ColumnName { get; set; }
        public bool? Idcolumn { get; set; }
        public string ColType { get; set; }
        public string DisplayName { get; set; }
        public bool? IsDisplayed { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsUnique { get; set; }
        public string Fktable { get; set; }
        public string FkjoinCol { get; set; }
        public string FkdisplayCol { get; set; }
        public int? PixelWidth { get; set; }
        public bool BasicField { get; set; }
        public bool GeneralSearch { get; set; }
    }
}
