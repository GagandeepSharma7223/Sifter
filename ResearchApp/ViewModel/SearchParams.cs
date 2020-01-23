using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.ViewModel
{
    public class SearchParams
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string ComparisonType { get; set; }
        public string TextValue { get; set; }
        public decimal? Number1 { get; set; }
        public decimal? Number2 { get; set; }
    }
}
