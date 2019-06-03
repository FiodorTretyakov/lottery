using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lottery.Models;

namespace Lottery.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketContext context;

        public TicketController(TicketContext c)
        {
            context = c;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> Get() => context.Tickets.ToList();

        [HttpGet("/{id}")]
        public async Task<ActionResult<Ticket>> Get(int id) =>
            await context.Tickets.FindAsync(id).ConfigureAwait(false);

        [HttpPost]
        public async Task<int> Post([FromBody] string value)
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

        [HttpPut("/{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                var ticket = await context.Tickets.FindAsync(id).ConfigureAwait(false);
                context.Lines.RemoveRange(context.Lines.Where(line => line.TicketId == id));
                ticket.Lines.Clear();

                ticket.Lines.ToList().AddRange(Ticket.DeserializeLines(value));

                await context.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
            }
        }
    }
}
