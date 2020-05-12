using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorServerApp.Models;
using Newtonsoft.Json;

namespace BlazorServerApp
{
    [Route("api/MarvelCharacter")]
    [ApiController]
    public class MarvelCharacterController : ControllerBase
    {
        // GET: api/MarvelCharacter
        [HttpGet]
        public async Task<IEnumerable<MarvelCharactersResult>> Get()
        {
            IEnumerable<MarvelCharactersResult> MarvelCharacters = new List<MarvelCharactersResult>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://gateway.marvel.com:443/v1/public/characters/1011334?apikey=e80cce8e7ae9c9ae7f98e5d5b007e961"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var character = JsonConvert.DeserializeObject<MarvelCharacters>(apiResponse);
                    //MarvelCharacters.Append(character);
                }

                using (var response = await httpClient.GetAsync("https://gateway.marvel.com:443/v1/public/characters/1017100?apikey=e80cce8e7ae9c9ae7f98e5d5b007e961"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var character = JsonConvert.DeserializeObject<MarvelCharacters>(apiResponse);
                    //MarvelCharacters.Append(character);
                }
            }
            return MarvelCharacters;
        }

        // GET: api/MarvelCharacter/5
        [HttpGet("{id}", Name = "GetMarvelCharacter")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MarvelCharacter
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MarvelCharacter/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
