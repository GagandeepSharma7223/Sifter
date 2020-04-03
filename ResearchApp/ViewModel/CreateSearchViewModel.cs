using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.ViewModel
{
    public class CreateSearchViewModel
    {
        public CreateSearchViewModel()
        {
            SearchParams = new List<SearchParams>();
        }
        public string Name { get; set; }
        public int MemberID { get; set; }
        public string SearchString { get; set; }
        public List<SearchParams> SearchParams { get; set; }
    }
}
