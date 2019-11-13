using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class UnitViewModel
    {
        public UnitViewModel()
        {
            Work = new DropdownOptions();
            Category = new DropdownOptions();
        }
        public int UnitID { get; set; }
        public int? WorkId { get; set; }
        public int? StartPage { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string LiteralTitle { get; set; }
        public string TitleEnglish { get; set; }
        public string Attribution { get; set; }

        [UIHint("ClientWork")]
        public DropdownOptions Work { get; set; }
        [UIHint("ClientCategory")]
        public DropdownOptions Category { get; set; }
    }
}
