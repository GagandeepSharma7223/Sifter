﻿using System.ComponentModel.DataAnnotations;
namespace ResearchApp.ViewModel
{
    public class AuthorViewModel
    {
        public int? AuthorID { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsOrganization { get; set; }
        [UIHint("ClientGender")]
        public string Gender { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthCountryID { get; set; }
        public string PenName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string Title { get; set; }
        public int? DeathYear { get; set; }
        public int? FirstActivityYear { get; set; }
        public string Occupation { get; set; }
        public string Sources { get; set; }
        public string Comments { get; set; }

        [UIHint("ClientCountry")]
        public DropdownOptions BirthCountry { get; set; }
    }
}
