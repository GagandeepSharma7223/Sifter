using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AdminHoneypotLoginattempt
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string IpAddress { get; set; }
        public string SessionKey { get; set; }
        public string UserAgent { get; set; }
        public string Timestamp { get; set; }
        public string Path { get; set; }
    }
}
