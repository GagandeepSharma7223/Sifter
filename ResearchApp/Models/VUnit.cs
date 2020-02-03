using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class VUnit
    {
        public int UnitId { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public string Author { get; set; }
        public int? StartPage { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public string Attribution { get; set; }
        //public int? WorkId { get; set; }
    }
}
