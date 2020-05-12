using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerApp
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "Name")]
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // GET: api/Demo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            if (identity != null)
            {
                //var name = identity.FindFirst("ClaimName").Value;
                var name = identity.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                return new string[] { "value1", "value2", name };
            }
            return new string[] { "value1", "value2" };
        }

        // GET: api/Demo/5
        [HttpGet("{id}", Name = "GetDemo")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Demo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Demo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //// DELETE: api/Demo/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
