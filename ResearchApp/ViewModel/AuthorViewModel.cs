using System.ComponentModel.DataAnnotations;
namespace ResearchApp.ViewModel
{
    public class AuthorViewModel
    {
        public int AuthorID { get; set; }
        [Required]
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [UIHint("ClientYesNo")]
        public TextDropdownOptions IsOrganization { get; set; }
        [UIHint("ClientGender")]
        public TextDropdownOptions Gender { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthCountryId { get; set; }
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
