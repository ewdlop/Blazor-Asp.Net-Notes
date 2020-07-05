using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorServerApp.Models.API.RPG
{
    public class Location
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        [Display(Name = "Monster ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(30)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public ICollection<MonsterResidency> Monsters { get; set; }
    }
}
