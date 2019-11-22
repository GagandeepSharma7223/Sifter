using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class RegionViewModel
    {
        public RegionViewModel()
        {
            Country = new DropdownOptions();
        }
        public int? RegionID { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public string DisplayName { get; set; }
        public string GeoNameCode { get; set; }
        public string AlternateNames { get; set; }
        public int? GeoNameId { get; set; }
        public string NameAscii { get; set; }
        public string Slug { get; set; }
        [UIHint("ClientCountry")]
        public DropdownOptions Country { get; set; }
    }
}
