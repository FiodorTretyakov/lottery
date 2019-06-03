using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<Ticket>>> Get() =>
            await context.Tickets.Include(t => t.Lines).ToListAsync()
            .ConfigureAwait(false);

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(int id)
        {
            var ticket = await context.Tickets.Include(t => t.Lines).FirstOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] string value)
        {
            using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                Ticket ticket;
                try
                {
                    ticket = new Ticket(value);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    return BadRequest(e);
                }

                await context.Tickets.AddAsync(ticket).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();

                return ticket.Id;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> Put(int id, [FromBody] string value)
        {
            using (var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false))
            {
                var ticket = await context.Tickets.FindAsync(id).ConfigureAwait(false);

                if (ticket == null)
                {
                    return NotFound();
                }

                context.Lines.RemoveRange(context.Lines.Where(line => line.TicketId == id));
                ticket.Lines.Clear();

                ticket.Lines.ToList().AddRange(Ticket.DeserializeLines(value));

                await context.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();

                return await context.Tickets.Include(t => t.Lines).FirstAsync(t => t.Id == id)
                    .ConfigureAwait(false);
            }
        }
    }
}
