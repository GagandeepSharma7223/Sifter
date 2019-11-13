using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationWorkKnownCopies
    {
        public int? Id { get; set; }
        public int? WorkId { get; set; }
        public int? RepositoryId { get; set; }
    }
}
