using ResearchApp.Models;
using System.Collections.Generic;

namespace ResearchApp.ViewModel
{
    public class AdvanceSearchViewModel
    {
        public AdvanceSearchViewModel()
        {
            Authors = new List<VAuthor>();
            Works = new List<VWork>();
            Units = new List<VUnit>();
        }
        public List<VAuthor> Authors { get; set; }
        public List<VWork> Works { get; set; }
        public List<VUnit> Units { get; set; }
        public List<TreeColumnViewModel> AuthorColumns { get; set; }
        public List<TreeColumnViewModel> WorkColumns { get; set; }
        public List<TreeColumnViewModel> UnitColumns { get; set; }
    }
}
