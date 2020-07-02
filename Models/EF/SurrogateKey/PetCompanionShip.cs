using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorServerApp.Models.EF.SurrogateKey
{
    public class PetCompanionShip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Owner Owner { get; set; }
        public Guid OwnerId { get; set; }
        public Pet Pet { get; set; }
        public Guid PetId { get; set; }
    }
}
