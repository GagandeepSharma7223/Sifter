﻿namespace ResearchApp.ViewModel
{
    public class SearchParams
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string ComparisonType { get; set; }
        public string TextValue { get; set; }
        public string AndOr { get; set; }
    }
}
