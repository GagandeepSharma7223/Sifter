using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class RawWork
    {
        public double? WorkId { get; set; }
        public double? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string TitleLit { get; set; }
        public string TitleStd { get; set; }
        public string TitleEng { get; set; }
        public string TitleAbbreviated { get; set; }
        public string DigitizedVersion { get; set; }
        public string Isbn { get; set; }
        public string Volumes { get; set; }
        public string PubPlaceLit { get; set; }
        public string PubPlaceEng { get; set; }
        public string Publisher { get; set; }
        public string PostPublicationHist { get; set; }
        public string SourceDocument { get; set; }
        public string PriceOfWork { get; set; }
        public string Languages { get; set; }
        public string Date { get; set; }
        public bool DatePrecise { get; set; }
        public string SortDate { get; set; }
        public string Translator { get; set; }
        public string Editor { get; set; }
        public string EditionNumber { get; set; }
        public string Comment { get; set; }
        public string ProvenanceOfSourceCopy { get; set; }
        public string CopiesKnown { get; set; }
        public string SourceCopy { get; set; }
        public string CountryOfPublication { get; set; }
        public string CollectiveWorkTitle { get; set; }
        public string Citations { get; set; }
        public string AuthorId2 { get; set; }
    }
}
