using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Lottery.Db;

namespace Lottery.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("/{id}")]
        public static async Task<ActionResult<string>> Get(int id)
        {
            using (var context = new TicketContext())
            {
                return JsonConvert.SerializeObject(await context.Tickets.FindAsync(id).ConfigureAwait(false));
            }
        }

        [HttpPost]
        public static async Task<int> Post([FromBody] string value)
        {
            using (var context = new TicketContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    var ticket = new Ticket(value);
                    await context.Tickets.AddAsync(ticket).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();

                    return ticket.Id;
                }
            }
        }

        [HttpPut("/{id}")]
        public static async Task Put(int id, [FromBody] string value)
        {
            using (var context = new TicketContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    var ticket = await context.Tickets.FindAsync(id).ConfigureAwait(false);
                    ticket.Lines.ToList().RemoveAll(line => true);
                    ticket.Lines.ToList().AddRange(Ticket.DeserializeLines(value));

                    await context.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                }
            }
        }
    }
}
