using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class UnitUnit
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string Title { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public int? AttributionId { get; set; }
        public int? CategoryId { get; set; }
        public string Comments { get; set; }
        public int? EndPage { get; set; }
        public string LiteralTitle { get; set; }
        public int? Order { get; set; }
        public int? RelatedWorkId { get; set; }
        public string RomanNumerals { get; set; }
        public int? StartPage { get; set; }
        public string Text { get; set; }
        public string EnglishTitle { get; set; }
    }
}
