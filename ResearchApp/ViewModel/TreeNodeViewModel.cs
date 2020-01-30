namespace ResearchApp.ViewModel
{
    public class TreeNodeViewModel
    {
        public int id { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
        public string Text { get; set; }
        public string tableName { get; set; }
    }
}
