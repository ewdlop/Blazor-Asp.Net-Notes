using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorServerApp.Models.API.RPG
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MonsterType
    {
        Creature,
        Elemental,
        Humanoid
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MonsteRarity
    {
        Boss,
        Elite,
        Normal
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttackType
    {
        Magic,
        Melee,
        Range
    }

    public class Monster
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        [Display(Name = "Monster ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [StringLength(30)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "type")]
        [Column(TypeName = "nvarchar(25)")]
        public MonsterType Type { get; set; }
        
        [JsonProperty(PropertyName = "rarity")]
        [Display(Name = "Monster Rarity")]
        [Column(TypeName = "nvarchar(15)")]

        public MonsteRarity Rarity { get; set; }

        [JsonProperty(PropertyName = "attack_type")]
        [Display(Name = "Attack Type")]
        [Column(TypeName = "nvarchar(15)")]
        public AttackType AttackType { get; set; }
        [JsonProperty(PropertyName = "spawn_locations")]
        public ICollection<MonsterResidency> SpawnLocations { get; set; }
    }
}
