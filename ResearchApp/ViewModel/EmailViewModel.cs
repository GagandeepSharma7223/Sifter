using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.ViewModel
{
    public class EmailViewModel
    {
        public string ToName { get; set; }
        public string FromName { get { return "Sifter"; } }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromPassword { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
