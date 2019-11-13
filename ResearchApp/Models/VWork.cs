using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class VWork
    {
        public int WorkId { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? BirthYear { get; set; }
        public string BirthCountry { get; set; }
        public string Language { get; set; }
        public string PubCity { get; set; }
        public string PubRegion { get; set; }
        public string PubCountry { get; set; }
        public string Publisher { get; set; }
        public string Editor { get; set; }
        public string Translator { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleLiteral { get; set; }
    }
}
