using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Region
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public string DisplayName { get; set; }
        public string GeoNameCode { get; set; }
        public string AlternateNames { get; set; }
        public int? GeoNameId { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        public virtual Country Country { get; set; }
    }
}
