using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Models.EF.NautralKey
{
    public enum MemberShipType
    {
        Free,
        Gold,
        platinum,
        Silver
    }
    public class Membership {
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public MemberShipType Type { get; set; }
        public double Fee { get; set; }
        public ICollection<CustomerMemberShip> OwnedByMembers { get; set; }
    }
}
