using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class Book2
    {
        public int Book2Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string PublicationDate { get; set; }
        public string FileLocation { get; set; }
        public string Extension { get; set; }
        public byte[] Contents { get; set; }
    }
}
