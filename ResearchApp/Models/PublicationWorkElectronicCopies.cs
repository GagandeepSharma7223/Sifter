using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class PublicationWorkElectronicCopies
    {
        public int? Id { get; set; }
        public int? WorkId { get; set; }
        public int? DigitalsurrogateId { get; set; }
    }
}
