using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthorAuthorLanguagesKnown
    {
        public int? Id { get; set; }
        public int? AuthorId { get; set; }
        public int? LanguageId { get; set; }
    }
}
