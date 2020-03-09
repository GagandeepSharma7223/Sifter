using System.Collections.Generic;

namespace ResearchApp.ViewModel
{
    public class FormViewModel
    {
        public dynamic SelectedItem { get; set; }
        public IList<TreeColumnViewModel> TableColumns { get; set; }
        public string FormName { get; set; }
    }
    public class TreeColumnViewModel
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int? ColSeq { get; set; }
        public bool IDColumn { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public bool? Display { get; set; }
        public bool? Editable { get; set; }
        public string Fktable { get; set; }
        public string FkjoinCol { get; set; }
        public string FkdisplayCol { get; set; }
        public bool IsEditable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
        public string ColType { get; set; }
        public int? PixelWidth { get; set; }
    }
}
