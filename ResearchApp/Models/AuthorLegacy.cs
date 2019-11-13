using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthorLegacy
    {
        public int? Id { get; set; }
        public int? LegacyId { get; set; }
        public string LegacyData { get; set; }
    }
}
