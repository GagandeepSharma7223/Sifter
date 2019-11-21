
namespace ResearchApp.Models
{
    public partial class Author
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsOrganization { get; set; }
        public string Gender { get; set; }
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

        public virtual Country BirthCountry { get; set; }
    }
}
