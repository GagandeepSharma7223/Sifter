using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class WorkViewModel
    {
        public WorkViewModel()
        {
            Author = new DropdownOptions();
            City = new DropdownOptions();
            Publisher = new DropdownOptions();
            Language = new DropdownOptions();
        }
        public int? WorkID { get; set; }
        public string Title { get; set; }
        public int? PublicationYear { get; set; }
        public int? AuthorID { get; set; }
        public int? LanguageID { get; set; }
        public int? CityID { get; set; }
        public int? PublisherID { get; set; }
        public string TitleEnglish { get; set; }
        public string TitleLiteral { get; set; }
        public int? TranslatorID { get; set; }
        public int? EditorID { get; set; }

        [UIHint("ClientAuthor")]
        public DropdownOptions Author { get; set; }
        [UIHint("ClientCity")]
        public DropdownOptions City { get; set; }
        [UIHint("ClientPublisher")]
        public DropdownOptions Publisher { get; set; }
        [UIHint("ClientLanguage")]
        public DropdownOptions Language { get; set; }

        [UIHint("ClientAuthor")]
        public DropdownOptions Translator { get; set; }

        [UIHint("ClientAuthor")]
        public DropdownOptions Editor { get; set; }

    }
}
