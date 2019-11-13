using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class RawAuthor
    {
        public double? AuthorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PenName { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string BirthDt { get; set; }
        public string BirthCity { get; set; }
        public string CountryOfBirth { get; set; }
        public string DeathDt { get; set; }
        public string PlaceOfDeath { get; set; }
        public string CountryOfDeath { get; set; }
        public string Affiliation { get; set; }
        public string MarriedTo { get; set; }
        public string WhenActive { get; set; }
        public string SortDate { get; set; }
        public string Comments { get; set; }
        public string Citations { get; set; }
        public string TextFromBibliography2002 { get; set; }
        public string Keyword { get; set; }
        public string DigitizedCopy { get; set; }
        public string LocationDigitizedTexts { get; set; }
    }
}
