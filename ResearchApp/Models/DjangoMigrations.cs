using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class DjangoMigrations
    {
        public int? Id { get; set; }
        public string App { get; set; }
        public string Name { get; set; }
        public string Applied { get; set; }
    }
}
