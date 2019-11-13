using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthorLanguage
    {
        public int AuthorLanguageId { get; set; }
        public int? AuthorId { get; set; }
        public int? LanguageId { get; set; }
    }
}
