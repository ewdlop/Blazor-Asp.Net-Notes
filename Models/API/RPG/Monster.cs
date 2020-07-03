using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorServerApp.Models.API.RPG
{
    public enum MonsterType
    {
        Creature,
        Elemental,
        Humanoid
    }

    public enum MonsteRarity
    {
        Boss,
        Elite,
        Normal
    }

    public enum AttackType
    {
        Magic,
        Melee,
        Range
    }

    public class Monster
    {
        [Key]
        [JsonPropertyName("id")]
        [Display(Name = "Monster ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [StringLength(30)]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("type")]
        [Column(TypeName = "nvarchar(25)")]
        public MonsterType Type { get; set; }
        
        [JsonPropertyName("rarity")]
        [Display(Name = "Monster Rarity")]
        [Column(TypeName = "nvarchar(15)")]

        public MonsteRarity Rarity { get; set; }

        [JsonPropertyName("attack_type")]
        [Display(Name = "Attack Type")]
        [Column(TypeName = "nvarchar(15)")]
        public AttackType AttackType { get; set; }

        public ICollection<MonsterResidency> SpawnLocations { get; set; }
    }
}
