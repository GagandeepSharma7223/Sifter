using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class PublisherViewModel
    {
        public int? PublisherID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
