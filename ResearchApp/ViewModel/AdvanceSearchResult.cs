using ResearchApp.Data;
using System.Collections.Generic;
using System.Data;

namespace ResearchApp.ViewModel
{
    public class AdvanceSearchResult
    {
        //public AdvanceSearchResult()
        //{
        //    this.Result = new List<T>();
        //}
        //public DataTable TableResult { get; set; }
        //public List<T> Result { get; set; }
        public string SearlizeResult { get; set; }
        public List<TreeColumnViewModel> Columns { get; set; }
        public int Total { get; set; }
        public GridTypes GridType { get; set; }
    }
}
