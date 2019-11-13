using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationLegacywork
    {
        public int? Id { get; set; }
        public int? LegacyId { get; set; }
        public int? LegacyAuthorId { get; set; }
        public string LegacyData { get; set; }
    }
}
