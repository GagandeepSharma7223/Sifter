using ResearchApp.Data;

namespace ResearchApp.ViewModel
{
    public class AdvanceSearchRequest
    {
        public string SearchType
        {
            get
            {
                switch (GridType)
                {
                    case GridTypes.VAuthor:
                        return "Authors";
                    case GridTypes.VWork:
                        return "Works";
                    case GridTypes.VUnit:
                        return "Elements";
                    default:
                        return string.Empty;
                }
            }
        }
        public bool CountOnly { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public GridTypes GridType { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}
