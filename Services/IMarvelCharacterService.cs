using BlazorServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public interface IMarvelCharacterService
    {
        Task<IEnumerable<MarvelCharactersResult>> GetMarvelCharactersAsync(int page);
        Task GetMoreMarvelCharacterAsync();
    }
}
