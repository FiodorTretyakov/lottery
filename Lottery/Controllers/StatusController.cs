using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lottery.Models;

namespace Lottery.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly TicketContext context;

        public StatusController(TicketContext c)
        {
            context = c;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> Put(int id)
        {
            var ticket = await context.Tickets.Include(t => t.Lines).FirstAsync(t => t.Id == id)
                .ConfigureAwait(false);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.IsChecked = true;
            await context.SaveChangesAsync().ConfigureAwait(false);

            return ticket;
        }
    }
}