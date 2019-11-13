using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class AuthUser
    {
        public int? Id { get; set; }
        public string Password { get; set; }
        public string LastLogin { get; set; }
        public string IsSuperuser { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IsStaff { get; set; }
        public string IsActive { get; set; }
        public string DateJoined { get; set; }
    }
}
