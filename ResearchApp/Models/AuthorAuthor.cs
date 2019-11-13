using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthorAuthor
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string IsOrganization { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string PenName { get; set; }
        public string AlsoKnownAs { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthDay { get; set; }
        public string BirthEra { get; set; }
        public int? DeathYear { get; set; }
        public int? DeathMonth { get; set; }
        public int? DeathDay { get; set; }
        public string DeathEra { get; set; }
        public int? ActivityStarts { get; set; }
        public string ActivityStartsEra { get; set; }
        public int? ActivityEnds { get; set; }
        public string ActivityEndsEra { get; set; }
        public string Sources { get; set; }
        public string Comments { get; set; }
        public string Accuracy { get; set; }
        public int? BirthCityId { get; set; }
        public int? BirthCountryId { get; set; }
        public int? BirthRegionId { get; set; }
        public int? CreatedById { get; set; }
        public int? DeathCityId { get; set; }
        public int? DeathCountryId { get; set; }
        public int? DeathRegionId { get; set; }
        public int? GenderId { get; set; }
        public int? ModifiedById { get; set; }
        public int? OccupationId { get; set; }
        public int? TitleId { get; set; }
        public string Spouse { get; set; }
        public string ValidName { get; set; }
    }
}
