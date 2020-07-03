using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorServerApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerApp
{
    public class DemoDTO
    {
        public int Id { get; set; }
        public string Output { get; set; }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = "Name")]
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // GET: api/Demo
        [HttpGet]
        public ActionResult<IEnumerable<DemoDTO>> Get()
        {
            List<DemoDTO> demos = new List<DemoDTO>();
            for(int i= 0; i < 10; i++) 
            {
                string output = string.Format("Demo item{0}", i);
                demos.Add(new DemoDTO { Id = i, Output = output});
            }
            return demos;
        }

        // GET: api/Demo/5
        [HttpGet("{id}")]
        public ActionResult<DemoDTO> Get(int id)
        {
            string output = string.Format("Demo item{0}", id);
            return new DemoDTO { Id = id, Output = output };
        }

        // POST: api/Demo
        [HttpPost]
        public ActionResult<DemoDTO> Post([FromBody] DemoDTO demo)
        {
            return CreatedAtAction(nameof(Get), new { id = demo.Id }, demo);
        }

        // PUT: api/Demo/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DemoDTO demo)
        {
            if(id != demo.Id) {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/Demo/5
        [HttpDelete("{id}")]
        public ActionResult<DemoDTO> Delete(int id)
        {
            string output = string.Format("Demo item{0}", id);
            return new DemoDTO { Id = id, Output = output };
        }
    }
}