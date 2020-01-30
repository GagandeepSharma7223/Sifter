using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Work
    {
        public int WorkID { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public int? AuthorID { get; set; }
        public int? LanguageID { get; set; }
        public int? CityID { get; set; }
        public int? PublisherID { get; set; }
        public int? EditorID { get; set; }
        public int? TranslatorID { get; set; }
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
