using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorServerApp.Data;
using Newtonsoft.Json;
using BlazorServerApp.Models.API.RPG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlazorServerApp.Controllers
{
    public class MonsterGetDTo
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public MonsterType Type { get; set; }

        [JsonProperty(PropertyName = "rarity")]
        public MonsteRarity Rarity { get; set; }

        [JsonProperty(PropertyName = "attack_type")]
        public AttackType AttackType { get; set; }

        [JsonProperty(PropertyName = "spawn_locations")]
        public ICollection<LocationGetDTO> SpawnLocations { get; set; }
    }

    public class MonsterPostDTo
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public MonsterType Type { get; set; }

        [JsonProperty(PropertyName = "rarity")]
        public MonsteRarity Rarity { get; set; }

        [JsonProperty(PropertyName = "attack_type")]
        public AttackType AttackType { get; set; }
    }

    public class LocationGetDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "respawn_time")]
        public float RespawnTime { get; set; }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Policy = "Name")]
    [Route("api/[controller]")]
    [ApiController]
    public class MonstersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MonstersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Monsters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonsterGetDTo>>> GetMonster()
        {
            return await _context.Monsters
                .Select(m => new MonsterGetDTo {
                    Id = m.Id,
                    Name = m.Name,
                    AttackType = m.AttackType,
                    Rarity = m.Rarity,
                    Type = m.Type,
                    SpawnLocations = m.SpawnLocations.Select(s => new LocationGetDTO {
                        RespawnTime = s.RespawnTime,
                        Id = s.LocationId,
                        Name = s.Location.Name
                    }).ToList()
                }).ToListAsync();
        }

        // GET: api/Monsters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonsterGetDTo>> GetMonster(int id)
        {
            var monster = await _context.Monsters.Where(m => m.Id == id)
            .Select(m => new MonsterGetDTo {
                Id = m.Id,
                Name = m.Name,
                AttackType = m.AttackType,
                Rarity = m.Rarity,
                Type = m.Type,
                SpawnLocations = m.SpawnLocations.Select(s => new LocationGetDTO {
                    RespawnTime = s.RespawnTime,
                    Id = s.LocationId,
                    Name = s.Location.Name
                }).ToList()
            }).FirstOrDefaultAsync();

            if (monster == null)
            {
                return NotFound();
            }

            return monster;
        }

        // PUT: api/Monsters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonster(int id, MonsterPostDTo monster)
        {
            if (id != monster.Id)
            {
                return BadRequest();
            }

            Monster newMonster = new Monster {
                Id = monster.Id,
                AttackType = monster.AttackType,
                Rarity = monster.Rarity,
                Name = monster.Name,
                Type = monster.Type
            };

            _context.Entry(newMonster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonsterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Monsters
        [HttpPost]
        public async Task<ActionResult<MonsterPostDTo>> PostMonster(MonsterPostDTo monster)
        {
            Monster newMonster = new Monster {
                Id = monster.Id,
                AttackType = monster.AttackType,
                Rarity = monster.Rarity,
                Name = monster.Name,
                Type = monster.Type
            };

            _context.Monsters.Add(newMonster);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MonsterExists(monster.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMonster", new { id = monster.Id }, monster);
        }

        // DELETE: api/Monsters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Monster>> DeleteMonster(int id)
        {

            return NotFound();

            //var monster = await _context.Monsters.FindAsync(id);
            //if (monster == null)
            //{
            //    return NotFound();
            //}

            //_context.Monsters.Remove(monster);
            //await _context.SaveChangesAsync();

            //return monster;
        }

        private bool MonsterExists(int id)
        {
            return _context.Monsters.Any(e => e.Id == id);
        }
    }
}
