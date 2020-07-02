using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Models.EF.NautralKey
{
    [Table("CustomerMembership")]
    public class CustomerMembership
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        public Guid MembershipId { get; set; }
        public DateTime MemberSince { get; set; }
        public Customer Customer { get; set; }
        public Membership Membership { get; set; }

    }
}
