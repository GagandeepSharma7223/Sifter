using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationWorkLanguage
    {
        public int? Id { get; set; }
        public int? WorkId { get; set; }
        public int? LanguageId { get; set; }
    }
}
