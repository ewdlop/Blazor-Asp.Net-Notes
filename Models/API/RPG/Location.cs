using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorServerApp.Models.API.RPG
{
    public class Location
    {
        [Key]
        [JsonPropertyName("id")]
        [Display(Name = "Monster ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(30)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public ICollection<MonsterResidency> Monsters { get; set; }
    }
}
