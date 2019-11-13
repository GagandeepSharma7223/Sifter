using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class VWorkAuthor
    {
        public int WorkAuthorId { get; set; }
        public string Work { get; set; }
        public string Author { get; set; }
        public string Role { get; set; }
    }
}
