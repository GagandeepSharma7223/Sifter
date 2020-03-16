using System;
using System.Collections.Generic;

namespace ResearchApp.Models
{
    public partial class MemberLogin
    {
        public int MemberLoginId { get; set; }
        public int? MemberId { get; set; }
        //public DateTime? LoginTime { get; set; }
        public string Ipaddress { get; set; }
    }
}
