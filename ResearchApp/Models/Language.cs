using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Language
    {
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime ValidFromUtc { get; set; }
        public DateTime ValidToUtc { get; set; }
    }
}
