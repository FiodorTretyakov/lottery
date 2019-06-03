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

        public TicketController(TicketContext c) => context = c;

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
            Ticket ticket;
            try
            {
                ticket = new Ticket(value);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

            await context.Tickets.AddAsync(ticket).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return ticket.Id;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] string value)
        {
            var ticket = await context.Tickets.Include(t => t.Lines).FirstOrDefaultAsync(t => t.Id == id)
                .ConfigureAwait(false);

            if (ticket == null)
            {
                return NotFound();
            }

            if (ticket.IsChecked)
            {
                return BadRequest("It is not possible to amend checked ticket.");
            }

            List<Line> lines;
            try
            {
                lines = Ticket.DeserializeLines(value);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

            if (ticket.Lines.Count != lines.Count)
            {
                return BadRequest(
                    $"Ticket created with {ticket.Lines.Count} lines, but try to amend with {lines.Count} lines.");
            }

            context.Lines.RemoveRange(context.Lines.Where(line => line.TicketId == id));
            ticket.Lines.Clear();
            ticket.Lines.AddRange(lines);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Ok();
        }
    }
}