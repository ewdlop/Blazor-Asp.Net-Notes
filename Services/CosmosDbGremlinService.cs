using Gremlin.Net.Driver;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class CosmosDbGremlinService : ICosmosDbGremlinService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CosmosDbGremlinService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SubmitQuery(string query)
        {
            try
            {
                using (var _gremlinClient = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IGremlinClient>())
                {
                    //Blazor componenet has problem getting this method for some reason
                    await _gremlinClient.SubmitAsync<dynamic>(query);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
