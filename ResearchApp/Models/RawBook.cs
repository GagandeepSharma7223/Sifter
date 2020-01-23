using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class RawBook
    {
        public string Authors { get; set; }
        public string PublicationDateStr { get; set; }
        public string Topics { get; set; }
        public string Publisher { get; set; }
        public string Collection { get; set; }
        public string Sponsor { get; set; }
        public string Contributor { get; set; }
        public string Languages { get; set; }
        public string LenTitle { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public string BookFrom { get; set; }
        public string FileBaseName { get; set; }
        public string FileCount { get; set; }
        public string ThumbFile { get; set; }
        public string MetaXmlfile { get; set; }
        public string Pdffile { get; set; }
        public string Bwpdffile { get; set; }
    }
}
