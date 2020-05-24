using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Models.EF.NautralKey
{
    [Table("Customer")]
    public class Customer
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        public ICollection<CustomerMembership> CustomerMemberShips { get; set; }
    }
}
