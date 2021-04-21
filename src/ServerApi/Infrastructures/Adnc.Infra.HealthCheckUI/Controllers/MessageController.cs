using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Adnc.Maintaining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        // GET: api/<NotifyController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<NotifyController>
        [HttpPost]
        public void Post([FromBody] JObject payload)
        {

        }

        // PUT api/<NotifyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] JObject payload)
        {
        }
    }
}
