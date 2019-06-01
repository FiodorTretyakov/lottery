using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Lottery.Db;

namespace Lottery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public static ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public static ActionResult<string> Get(int id)
        {
            return "value";
        }

        private ICollection<IList<int>> GetLines(string data) =>
            JsonConvert.DeserializeObject<IList<int>>(data);

        [HttpPost]
        public static void Post([FromBody] string value)
        {
            using (var context = new TicketContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var ticket = new Ticket(GetLines(value));
                    context.Tickets.Add(ticket);
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public static void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public static void Delete(int id)
        {
        }
    }
}
