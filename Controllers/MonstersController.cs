using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorServerApp.Data;
using Newtonsoft.Json;
using BlazorServerApp.Models.API.RPG;

namespace BlazorServerApp.Controllers
{
    public class MonsterDTO
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
        public ICollection<LocationDTO> SpawnLocations { get; set; }
    }

    public class LocationDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "respawn_time")]
        public float RespawnTime { get; set; }
    }

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
        public async Task<ActionResult<IEnumerable<MonsterDTO>>> GetMonster()
        {
            return await _context.Monsters
                .Select(m => new MonsterDTO {
                    Id = m.Id,
                    Name = m.Name,
                    AttackType = m.AttackType,
                    Rarity = m.Rarity,
                    Type = m.Type,
                    SpawnLocations = m.SpawnLocations.Select(s => new LocationDTO {
                        RespawnTime = s.RespawnTime,
                        Id = s.LocationId,
                        Name = s.Location.Name
                    }).ToList()
                }).ToListAsync();
        }

        // GET: api/Monsters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonsterDTO>> GetMonster(int id)
        {
            var monster = await _context.Monsters.Where(m => m.Id == id)
            .Select(m => new MonsterDTO {
                Id = m.Id,
                Name = m.Name,
                AttackType = m.AttackType,
                Rarity = m.Rarity,
                Type = m.Type,
                SpawnLocations = m.SpawnLocations.Select(s => new LocationDTO {
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
        public async Task<IActionResult> PutMonster(int id, Monster monster)
        {
            return NotFound();

            if (id != monster.Id)
            {
                return BadRequest();
            }

            _context.Entry(monster).State = EntityState.Modified;

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
        public async Task<ActionResult<Monster>> PostMonster(Monster monster)
        {
            return NotFound();

            _context.Monsters.Add(monster);
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

            var monster = await _context.Monsters.FindAsync(id);
            if (monster == null)
            {
                return NotFound();
            }

            _context.Monsters.Remove(monster);
            await _context.SaveChangesAsync();

            return monster;
        }

        private bool MonsterExists(int id)
        {
            return _context.Monsters.Any(e => e.Id == id);
        }
    }
}
