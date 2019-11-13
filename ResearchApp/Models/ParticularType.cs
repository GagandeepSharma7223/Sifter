using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class ParticularType
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string Name { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public int? Level { get; set; }
        public int? Lft { get; set; }
        public int? ParentId { get; set; }
        public int? Rght { get; set; }
        public int? TreeId { get; set; }
    }
}
