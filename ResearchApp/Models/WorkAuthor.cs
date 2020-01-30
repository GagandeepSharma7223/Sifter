using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class WorkAuthor
    {
        public int WorkAuthorID { get; set; }
        public int? WorkID { get; set; }
        public int? AuthorID { get; set; }
        public string Role { get; set; }

        public virtual Work Work { get; set; }
        public virtual Author Author { get; set; }
    }
}
