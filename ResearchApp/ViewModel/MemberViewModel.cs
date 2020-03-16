using System;

namespace ResearchApp.ViewModel
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? SuperUser { get; set; }
        public bool? AdminUser { get; set; }
        public DateTime? Created { get; set; }
        public string UserName { get; set; }
        public DateTime ValidFromUtc { get; set; }
        public DateTime ValidToUtc { get; set; }
    }
}
