using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class WorkAuthorViewModel
    {
        public WorkAuthorViewModel()
        {
            Work = new DropdownOptions();
            Author = new DropdownOptions();
        }
        public int WorkAuthorID { get; set; }
        public int? WorkID { get; set; }
        public int? AuthorID { get; set; }
        public string Role { get; set; }

        [UIHint("ClientAuthor")]
        public DropdownOptions Author { get; set; }
        [UIHint("ClientWork")]
        public DropdownOptions Work { get; set; }
    }
}
