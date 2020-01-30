using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Region
    {
        public int RegionID { get; set; }
        public string Name { get; set; }
        public int? CountryID { get; set; }
        public string DisplayName { get; set; }
        public string GeoNameCode { get; set; }
        public string AlternateNames { get; set; }
        public int? GeoNameID { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        public virtual Country Country { get; set; }
    }
}
