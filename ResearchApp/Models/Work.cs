using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Work
    {
        public int WorkId { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public int? AuthorId { get; set; }
        public int? LanguageId { get; set; }
        public int? CityId { get; set; }
        public int? PublisherId { get; set; }
        public int? EditorId { get; set; }
        public int? TranslatorId { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleLiteral { get; set; }

        public virtual Author Author { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Language Language { get; set; }
        public virtual City City { get; set; }
        public virtual Author Translator { get; set; }
        public virtual Author Editor { get; set; }
    }
}
