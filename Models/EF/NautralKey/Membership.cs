using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Table("Membership")]
    public class Membership 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(25)")]
        public MemberShipType Type { get; set; }
        [Required]
        [Range(0, 9999.99)]
        public decimal Fee { get; set; }
        public ICollection<CustomerMembership> OwnedByMembers { get; set; }
    }
}
