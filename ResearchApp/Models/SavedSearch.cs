using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class SavedSearch
    {
        public int SavedSearchId { get; set; }
        public int? MemberId { get; set; }
        public string Name { get; set; }
        public DateTime? SaveTime { get; set; }
        public string SearchString { get; set; }
    }
}
