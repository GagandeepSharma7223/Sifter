using Kendo.Mvc;

namespace ResearchApp.ViewModel
{
    public class StringCompareRequest
    {
        public string Entity { get; set; }
        public string Value { get; set; }
        public string FKColumn { get; set; }
        public FilterOperator Operator { get; set; }
    }
}
