using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Country
    {
        public int CountryID { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public string AlternateNames { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string TlD { get; set; }
        public string Phone { get; set; }
        public int? GeoNameID { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
    }
}
