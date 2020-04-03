using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class SavedSearchParam
    {
        public int SavedSearchParamId { get; set; }
        public int? SavedSearchId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string ComparisonType { get; set; }
        public string TextValue { get; set; }
        public string AndOr { get; set; }
    }
}
