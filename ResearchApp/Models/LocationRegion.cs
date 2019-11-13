using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class LocationRegion
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        public int? GeonameId { get; set; }
        public string AlternateNames { get; set; }
        public string DisplayName { get; set; }
        public string GeonameCode { get; set; }
        public int? CountryId { get; set; }
    }
}
