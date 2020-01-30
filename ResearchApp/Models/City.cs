using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class City
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? CountryID { get; set; }
        public int? RegionID { get; set; }
        public int? Population { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string TimeZone { get; set; }
        public string FeatureCode { get; set; }
        public int? GeoNameID { get; set; }
        public string AlternateNames { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        public string SearchNames { get; set; }
        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
    }
}
