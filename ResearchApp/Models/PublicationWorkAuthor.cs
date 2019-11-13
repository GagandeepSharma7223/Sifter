using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationWorkAuthor
    {
        public int? Id { get; set; }
        public int? WorkId { get; set; }
        public int? ContributorId { get; set; }
    }
}
