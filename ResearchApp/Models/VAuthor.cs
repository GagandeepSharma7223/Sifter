using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class VAuthor
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IsOrganization { get; set; }
        public string Gender { get; set; }
        public int? BirthYear { get; set; }
        public string BirthCountry { get; set; }
        public string PenName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string Title { get; set; }
        public int? DeathYear { get; set; }
        public int? FirstActivityYear { get; set; }
        public string Occupation { get; set; }
        public string Sources { get; set; }
        public string Comments { get; set; }
    }
}
