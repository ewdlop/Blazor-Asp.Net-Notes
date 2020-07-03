using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorServerApp.Models.API.RPG
{
    public class MonsterResidency
    {
        [Key]
        [JsonPropertyName("id")]
        [Display(Name = "Monster ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        public Monster Monster { get; set; }
        public int MonsterId { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        [JsonPropertyName("respawn_time")]
        [Display(Name = "Respawn Time")]
        public float RespawnTime { get; set; }
    }
}