using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Models.EF.NautralKey
{
    public class CustomerMemberShip
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MembershipId { get; set; }
        public DateTime MemberSince { get; set; }
        public Customer Customer { get; set; }
        public Membership Membership { get; set; }

    }
}
