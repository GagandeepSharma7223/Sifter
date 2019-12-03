using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class CityViewModel
    {
        public CityViewModel()
        {
            Country = new DropdownOptions();
            Region = new DropdownOptions();
        }
        public int? CityID { get; set; }
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

        [UIHint("ClientCountry")]
        public DropdownOptions Country { get; set; }
        [UIHint("ClientRegion")]
        public DropdownOptions Region { get; set; }
    }
}
