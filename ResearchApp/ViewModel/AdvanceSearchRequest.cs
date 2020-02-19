using ResearchApp.Data;

namespace ResearchApp.ViewModel
{
    public class AdvanceSearchRequest
    {
        public string TableName { get; set; }
        public bool IsView { get; set; }
        public bool CountOnly { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}
