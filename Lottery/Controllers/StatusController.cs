using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut("/{id}")]
        public async Task Put(int id)
        {
            var ticket = await context.Tickets.FindAsync(id).ConfigureAwait(false);
            ticket.IsChecked = true;
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}