using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthorContributor
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public int? NameId { get; set; }
        public int? RoleId { get; set; }
    }
}
