using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationWork
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public int? CategoryId { get; set; }
        public string Citations { get; set; }
        public string Comments { get; set; }
        public int? CurrencyId { get; set; }
        public int? EditionNumber { get; set; }
        public string Isbn { get; set; }
        public int? Pages { get; set; }
        public string PublicationEra { get; set; }
        public string PublicationHistory { get; set; }
        public int? PublicationPlaceId { get; set; }
        public int? PublicationYear { get; set; }
        public int? PublisherId { get; set; }
        public int? SeriesId { get; set; }
        public string Sources { get; set; }
        public string Title { get; set; }
        public string TitleAbbreviated { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleLiteral { get; set; }
        public double? Value { get; set; }
        public int? Volume { get; set; }
        public int? Volumes { get; set; }
    }
}
