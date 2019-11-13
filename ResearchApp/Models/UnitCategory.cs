﻿using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class UnitCategory
    {
        public int? Id { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string Name { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public string Notes { get; set; }
    }
}
