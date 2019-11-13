using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ServicesHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        protected readonly List<string> data;
        public ValuesController()
        {
            data = Enumerable.Range(1, 50).Select(e => $"value {e}").ToList();
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return data;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id > data.Count | id < 0)
                return NotFound();
            return data[id];
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            data.Add(value);
            return Ok($"api/values/{data.Count - 1}");
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            try
            {
                data.Insert(id, value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var item = data[id];
                data.Remove(item);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
