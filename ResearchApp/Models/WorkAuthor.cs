using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class WorkAuthor
    {
        public int WorkAuthorId { get; set; }
        public int? WorkId { get; set; }
        public int? AuthorId { get; set; }
        public string Role { get; set; }

        public virtual Work Work { get; set; }
        public virtual Author Author { get; set; }
    }
}
