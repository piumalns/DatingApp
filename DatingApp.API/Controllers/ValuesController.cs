using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    //hhtp://localhost:5000/api/values
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            //throw new Exception("test excetion");
            _context = context;
        }

        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var values = _context.values.ToList();
            return Ok(values);
        }

        // GET api/values/5
        // [AllowAnonymous] that time 
        [HttpGet("{id}")]
        public IActionResult Get(int id)    
        {
            var value = _context.values.FirstOrDefault(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
