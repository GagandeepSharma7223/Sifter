using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class VUnit
    {
        public int UnitId { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public string FullName { get; set; }
        public int? StartPage { get; set; }
        public string Category { get; set; }
        public string UnitText { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? BirthYear { get; set; }
        public string BirthCountry { get; set; }
        public string Language { get; set; }
        public string PubCity { get; set; }
        public string PubRegion { get; set; }
        public string PubCountry { get; set; }
        public string Publisher { get; set; }
        public string UnitLiteralTitle { get; set; }
        public string UnitTitle { get; set; }
        public string UnitTitleEnglish { get; set; }
        public string Attribution { get; set; }
    }
}
