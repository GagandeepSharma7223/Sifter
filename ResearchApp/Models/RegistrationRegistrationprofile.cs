using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class RegistrationRegistrationprofile
    {
        public int? Id { get; set; }
        public string ActivationKey { get; set; }
        public int? UserId { get; set; }
        public string Activated { get; set; }
    }
}
