using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class RawUnit
    {
        public double? UnitId { get; set; }
        public string ShortName { get; set; }
        public string WorkId { get; set; }
        public string UnitIdReal { get; set; }
        public string UnitTitleLit { get; set; }
        public string UnitTitleStd { get; set; }
        public string UnitTitleEnglish { get; set; }
        public string AuthorName { get; set; }
        public string InfoType { get; set; }
        public string Comments { get; set; }
        public string TextAndWork { get; set; }
        public double? NumbersAndWork { get; set; }
        public string LocatorId { get; set; }
        public string Page { get; set; }
        public string UnitPartField { get; set; }
        public string Order { get; set; }
        public string Text { get; set; }
        public string Attribution { get; set; }
    }
}
