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
        public bool IsAdvanceSearch { get; set; }
        public bool IsGlobalSearch { get; set; }
        public string SearchText { get; set; }
        public bool GetParamTable { get; set; }
        public string FromClause
        {
            get
            {
                return IsAdvanceSearch ?
                    " from vAuthor left outer join vWork on vWork.AuthorID = vAuthor.AuthorID left outer join vElement on vElement.WorkID = vWork.WorkID " :
                    string.Empty;
            }
        }
    }
}
