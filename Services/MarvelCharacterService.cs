using BlazorServerApp.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class MarvelCharacterService : IMarvelCharacterService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICosmosDbService<MarvelCharactersResult> _cosmosDbService;
        private IMemoryCache _cache;
        public MarvelCharacterService(ICosmosDbService<MarvelCharactersResult> cosmosDbService, IMemoryCache cache, IHttpClientFactory ClientFactory)
        {
            _clientFactory = ClientFactory;
            _cosmosDbService = cosmosDbService;
            _cache = cache;
        }
        public async Task<IEnumerable<MarvelCharactersResult>> GetMarvelCharactersAsync(int i)
        {
            string key = "characters" + i.ToString();
            if (!_cache.TryGetValue(key, out IEnumerable<MarvelCharactersResult> characters))
            {
                characters = await _cosmosDbService.GetItemsAsync(i);
                _cache.Set(key, characters);
            }
            //try
            //{
            //    characters = await _sessionStorage.GetItemAsync<IEnumerable<MarvelCharactersResult>>("characters");
            //    if(characters == null)
            //    {
            //        characters = await _cosmosDbService.GetItemsAsync();
            //        await _sessionStorage.SetItemAsync("characters", characters);
            //    }
            //}
            //catch
            //{
            //    characters = await _cosmosDbService.GetItemsAsync();
            //    await _sessionStorage.SetItemAsync("characters", characters);
            //}
            return characters;
        }

        public async Task GetMoreMarvelCharacterAsync()
        {
            //https://developer.marvel.com/
            List<MarvelCharactersResult> MarvelCharacters = new List<MarvelCharactersResult>();
            const string publicKey = "publicKey";
            const string privateKey = "privateKey";

            using (var httpClient = _clientFactory.CreateClient())
            {
                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                MD5 md5 = MD5.Create();
                string input = timestamp + privateKey + publicKey;
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                //The portal uses MD5 apparently?
                byte[] hash = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                string hashString = sb.ToString();
                //500
                const int limit = 50;
                const int offset = 50;
                string url = $"https://gateway.marvel.com:443/v1/public/characters?limit={limit}&offset={offset}&ts={timestamp}&apikey={publicKey}&hash={hashString}";
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var characters = JsonConvert.DeserializeObject<MarvelCharactersAPI>(apiResponse);
                    foreach(var character in characters.data.results)
                    {
                        MarvelCharacters.Add(new MarvelCharactersResult
                        {
                            api_id = character.id.Value.ToString(),
                            id = character.name,
                            comics = character.comics,
                            description = character.description,
                            events = character.events,
                            modified = character.modified,
                            resourceURI = character.resourceURI,
                            series = character.series,
                            stories = character.stories,
                            thumbnail = character.thumbnail,
                            urls = character.urls
                        });
                    }
                }
            }
            foreach (var character in MarvelCharacters)
            {
                await _cosmosDbService.AddItemAsync(character);
            }
        }
    }
}
