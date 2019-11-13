using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationRepository
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string Name { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public string Category { get; set; }
        public int? CountryId { get; set; }
        public string Url { get; set; }
    }
}
