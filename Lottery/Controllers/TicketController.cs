using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Lottery.Db;

namespace Lottery.Controllers
{
    [Route("ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        [HttpGet]
        public static ActionResult<string> Get()
        {
            using (var context = new TicketContext())
            {
                return JsonConvert.SerializeObject(context.Tickets.ToList());
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public static ActionResult<string> Get(int id)
        {
            return "value";
        }

        private static ICollection<IList<int>> GetLines(string data) =>
            JsonConvert.DeserializeObject<ICollection<IList<int>>>(data);

        [HttpPost]
        public static async Task Post([FromBody] string value)
        {
            using (var context = new TicketContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    var ticket = new Ticket(GetLines(value));
                    await context.Tickets.AddAsync(ticket).ConfigureAwait(false);
                    await context.Lines.AddRangeAsync(ticket.Lines).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
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
