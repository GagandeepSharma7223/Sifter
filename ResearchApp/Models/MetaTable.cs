using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class MetaTable
    {
        public int? MetaTableId { get; set; }
        public int? MetaCategoryId { get; set; }
        public int? TableSeq { get; set; }
        public string TableName { get; set; }
        public string DisplayName { get; set; }
        public bool? IsEditable { get; set; }
    }
}
