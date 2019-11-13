using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchApp.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleLiteral { get; set; }
        public int? PublicationYear { get; set; }
        public int? LocationID { get; set; }
        public int? PublisherID { get; set; }
    }
}
