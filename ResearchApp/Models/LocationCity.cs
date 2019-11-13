using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class LocationCity
    {
        public int Id { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        public int? GeonameId { get; set; }
        public string AlternateNames { get; set; }
        public string DisplayName { get; set; }
        public string SearchNames { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? Population { get; set; }
        public string FeatureCode { get; set; }
        public string Timezone { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
    }
}
